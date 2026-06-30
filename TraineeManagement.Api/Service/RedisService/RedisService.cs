using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace TraineeManagement.Api.Service.RedisService;

public class RedisService(IDistributedCache db, ILogger<RedisService> logger) : IRedisService
{
    private readonly IDistributedCache _db = db;
    private readonly ILogger<RedisService> _logger = logger;

    public async Task<T?> GetAsync<T>(string key)
    {
        try
        {
            var value = await _db.GetStringAsync(key);
            if (string.IsNullOrEmpty(value))
            {
                return default;
            }
            return JsonSerializer.Deserialize<T>(value);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while retrieveing data from the cache for the key - " + key + ex);
            return default;
        }
    }

    public async Task RemoveAsync(string key)
    {
        try
        {
            await _db.RemoveAsync(key);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while removing data from the cache for the key - " + key + ex);
        }
    }

    public async Task SetAsync<T>(string key, T value, double? expiry = null)
    {
        try
        {
            var json = JsonSerializer.Serialize(value);
            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiry ?? 10)
            };
            await _db.SetStringAsync(key, json, options);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while saving data from the cache for the key - " + key + ex);
        }
    }

}