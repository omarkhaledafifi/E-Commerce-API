using Services.Specifications;

namespace Services
{
    internal class OrderService(IUnitOfWork unitOfWork,
        IMapper mapper,
        IBasketRepository basketRepository)
        : IOrderService
    {
        public async Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail)
        {
            //1.  Address 

            var address = mapper.Map<OrderAddress>(request.ShipToAddress);
            //2. order Items => Basket => Basket items => order Items 

            var basket = await basketRepository.GetBasketAsync(request.BasketId)
                ?? throw new BasketNotFoundException(request.BasketId);

            var orderItems = new List<OrderItem>();
            foreach (var item in basket.Items)
            {
                var product = await unitOfWork.GetRepository<Product, int>()
                    .GetAsync(item.Id) ?? throw new ProductNotFoundException(item.Id);

                orderItems.Add(CreateOrderItem(item, product));
            }

            //3. Delivery 
            var deliveryMethod = await unitOfWork.GetRepository<DeliveryMethod, int>()
                .GetAsync(request.DeliveryMethodId)
                ?? throw new DeliverMethodNotFoundException(request.DeliveryMethodId);


            //4. SubTotal 

            var subtotal = orderItems.Sum(item => item.Price * item.Quantity);

            var repo = unitOfWork.GetRepository<Order, Guid>();
            var existingOrder = await
                repo.GetAsync(new OrderWithPaymentIntentSpecifications(basket.PaymentIntentId!));

            // save to db

            if (existingOrder is not null)
                repo.Delete(existingOrder);


            var order = new Order(userEmail, address, orderItems, deliveryMethod, subtotal, basket.PaymentIntentId!);

            await repo.AddAsync(order);
            await unitOfWork.SaveChangesAsync();

            // map & return 

            return mapper.Map<OrderResult>(order);
        }

        private OrderItem CreateOrderItem(BasketItem item, Product product)
            => new OrderItem(new ProductInOrderItem(product.Id, product.Name, product.PictureUrl),
                item.Quantity, product.Price);

        public async Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync()
        {
            var methods = await unitOfWork.GetRepository<DeliveryMethod, int>()
                 .GetAllAsync();
            return mapper.Map<IEnumerable<DeliveryMethodResult>>(methods);
        }

        public async Task<OrderResult> GetOrderByIdAsync(Guid id)
        {
            var order = await unitOfWork.GetRepository<Order, Guid>()
                .GetAsync(new OrderWithIncludeSpecifications(id))
                ?? throw new OrderNotFoundException(id);

            return mapper.Map<OrderResult>(order);
        }

        public async Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email)
        {
            var orders = await unitOfWork.GetRepository<Order, Guid>()
               .GetAllAsync(new OrderWithIncludeSpecifications(email));

            return mapper.Map<IEnumerable<OrderResult>>(orders);

        }
    }
}
