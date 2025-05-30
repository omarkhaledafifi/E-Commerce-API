namespace Services.Abstractions
{
    public interface ICacheService
    {
        public Task SetCacheValue(string key, object value, TimeSpan duration);

        public Task<string?> GetCachedItem(string key);
    }
}
