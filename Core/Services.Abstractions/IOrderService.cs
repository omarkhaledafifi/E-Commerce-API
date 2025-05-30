global using Shared.OrderModels;

namespace Services.Abstractions
{
    public interface IOrderService
    {


        //  Get Order By Id => OrderResult(Guid id)
        public Task<OrderResult> GetOrderByIdAsync(Guid id);
        //  get Orders for user By Email => IEnumerable<OrderResult>(string  email)
        public Task<IEnumerable<OrderResult>> GetOrdersByEmailAsync(string email);

        // Create Order  => OrderResult(OrderRequest  , string email )
        public Task<OrderResult> CreateOrderAsync(OrderRequest request, string userEmail);

        // Get All Delivery Methods 
        public Task<IEnumerable<DeliveryMethodResult>> GetDeliveryMethodsAsync();

    }
}
