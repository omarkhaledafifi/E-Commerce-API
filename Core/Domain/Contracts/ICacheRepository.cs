namespace Domain.Contracts
{
    public interface ICacheRepository
    {
        public Task SetAsync(string key, object value, TimeSpan duration);
        public Task<string?> GetAsync(string key);
    }
}
