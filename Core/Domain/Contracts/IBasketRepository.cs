using Domain.Entities;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        public Task<CustomerBasket?> GetBasketAsync(string id);

        public Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null); // TTL 

        public Task<bool> DeleteBasketAsync(string id);
    }
}
