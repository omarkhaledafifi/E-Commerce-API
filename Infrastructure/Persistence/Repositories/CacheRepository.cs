

using StackExchange.Redis;
using System.Text.Json;

namespace Persistence.Repositories
{
    public class CacheRepository(IConnectionMultiplexer connectionMultiplexer)
        : ICacheRepository
    {
        private readonly IDatabase _database = connectionMultiplexer.GetDatabase();
        public async Task<string?> GetAsync(string key)
        {
            var value = await _database.StringGetAsync(key);

            return !value.IsNullOrEmpty ? value : default;
        }

        public async Task SetAsync(string key, object value, TimeSpan duration)
        {
            var serializerObject = JsonSerializer.Serialize(value);

            await _database.StringSetAsync(key, serializerObject, duration);
        }
    }
}
