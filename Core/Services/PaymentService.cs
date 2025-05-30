
global using Domain.Entities.OrderEntities;
global using Microsoft.Extensions.Configuration;
global using Stripe;
global using Product = Domain.Entities.Product;
using Services.Specifications;
namespace Services
{
    class PaymentService(IBasketRepository basketRepository,
        IUnitOfWork unitOfWork,
        IConfiguration configuration,
        IMapper mapper)
        : IPaymentService
    {
        public async Task<BasketDTO> CreateOrUpdatePaymentIntent(string basketId)
        {

            StripeConfiguration.ApiKey = configuration.GetRequiredSection("StripeSettings")["SecretKey"];
            //StripeConfiguration.ApiKey = configuration["StripeSettings:SecretKey"];
            // get Basket 

            var basket = await basketRepository.GetBasketAsync(basketId)
                ?? throw new BasketNotFoundException(basketId);

            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                        .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                item.Price = product.Price;
            }


            if (!basket.DeliveryMethodId.HasValue) throw new Exception("No delivery Method was Selected");

            var method = await unitOfWork.GetRepository<DeliveryMethod, int>()
                       .GetAsync(basket.DeliveryMethodId.Value)
                       ?? throw new ProductNotFoundException(basket.DeliveryMethodId.Value);
            basket.ShippingPrice = method.Price;

            var service = new PaymentIntentService();

            var amount = (long)(basket.Items.Sum(i => i.Quantity * i.Price) + basket.ShippingPrice) * 100;


            if (string.IsNullOrWhiteSpace(basket.PaymentIntentId))
            {
                // Create 
                var createOptions = new PaymentIntentCreateOptions
                {
                    Amount = amount,
                    Currency = "USD",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                //service.CreateAsync();
                var paymentIntent = await service.CreateAsync(createOptions);

                basket.PaymentIntentId = paymentIntent.Id;
                basket.ClientSecret = paymentIntent.ClientSecret;
            }
            else
            {
                // Update 
                var updateOptions = new PaymentIntentUpdateOptions
                {
                    Amount = amount
                };
                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);

            }

            await basketRepository.UpdateBasketAsync(basket);

            return mapper.Map<BasketDTO>(basket);

        }

        public async Task UpdateOrderPaymentStatus(string request, string header)
        {
            var endPointSecret = configuration.GetRequiredSection("StripeSettings")["EndPointSecret"];
            var stripeEvent = EventUtility.ConstructEvent(request,
                header, endPointSecret);

            var paymentIntent = stripeEvent.Data.Object as PaymentIntent;
            switch (stripeEvent.Type)
            {
                case "payment_intent.payment_failed":
                    await UpdatePaymentFailed(paymentIntent!.Id);

                    break;
                case "payment_intent.succeeded":
                    await UpdatePaymentReceived(paymentIntent!.Id);
                    break;
                // ... handle other event types
                default:
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                    break;
            }
        }

        private async Task UpdatePaymentFailed(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithPaymentIntentSpecifications(paymentIntentId))
                ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentFailed;
            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync();
        }


        private async Task UpdatePaymentReceived(string paymentIntentId)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithPaymentIntentSpecifications(paymentIntentId))
                ?? throw new Exception();

            order.PaymentStatus = OrderPaymentStatus.PaymentReceived;

            unitOfWork.GetRepository<Order, Guid>().Update(order);
            await unitOfWork.SaveChangesAsync();
        }

    }
}
