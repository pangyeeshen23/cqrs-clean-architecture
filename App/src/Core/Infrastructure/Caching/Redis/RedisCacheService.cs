using System.Text.Json;
using Domain.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Infrastructure.Caching.Redis
{
    public class RedisCacheService : ICacheService
    {
        private readonly IDistributedCache _distributeCache;
        private readonly ILogger<RedisCacheService> _logger;
        private readonly JsonSerializerOptions _jsonOptions;
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _db;
        public RedisCacheService(
            IDistributedCache distributeCache,
            ILogger<RedisCacheService> logger,
            IConnectionMultiplexer connectionMultiplexer
        )
        {
            _distributeCache = distributeCache;
            _logger = logger;
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = false
            };
            _redis = connectionMultiplexer;
            _db = _redis.GetDatabase();
        }

        public async Task<T?> GetAsync<T>(string key, CancellationToken cancellationToken) where T : class
        {
            try
            {
                RedisValue cachedData = await _db.StringGetAsync(key);
                if (!cachedData.HasValue) return null;
                return JsonSerializer.Deserialize<T>(cachedData.ToString(), _jsonOptions);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting cache for key: {Key}", key);
                return null;
            }
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? expiration = null, CancellationToken cancellationToken = default)
        {
            try
            {
                var serialized = JsonSerializer.Serialize(value, _jsonOptions);
                await _db.StringSetAsync(key, serialized, TimeSpan.FromMinutes(5));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while setting cache for key: {Key}", key);
            }
        }

        public async Task RemoveAsync(string key, CancellationToken cancellationToken = default)
        {
            try
            {
                await _db.KeyDeleteAsync(key);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing cache for key: {Key}", key);
            }
        }

        public async Task RemoveItemsByPatternAsync(string pattern)
        {
            try
            {
                var server = _redis.GetServer(_redis.GetEndPoints().First());
                var keys = server.Keys(pattern: pattern);
                foreach (RedisKey key in keys)
                {
                    await _db.KeyDeleteAsync(key);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while removing cache for Pattern: {pattern}", pattern);
            }
        }
    }
}
