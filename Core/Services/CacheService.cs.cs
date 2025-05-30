
namespace Services
{
    class CacheService(ICacheRepository cacheRepository)
        : ICacheService
    {
        public async Task<string?> GetCachedItem(string key)
        {
            var value = await cacheRepository.GetAsync(key);
            return value == null ? null : value;
        }

        public async Task SetCacheValue(string key, object value, TimeSpan duration)
        {
            await cacheRepository.SetAsync(key, value, duration);
        }
    }
}
