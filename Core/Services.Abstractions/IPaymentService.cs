global using Shared;

namespace Services.Abstractions
{
    public interface IPaymentService
    {
        public Task<BasketDTO> CreateOrUpdatePaymentIntent(string basketId);
        public Task UpdateOrderPaymentStatus(string request, string header);
    }
}
