using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;

namespace traineeManagementAPI.Service.RedisService;

public class RedisService(IDistributedCache db): IRedisService
{
    private readonly IDistributedCache _db = db;

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.GetStringAsync(key);
        if (string.IsNullOrEmpty(value))
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value);
    }

    public async Task RemoveAsync(string key)
    {
        await _db.RemoveAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, double? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(expiry ?? 10)
        };
        await _db.SetStringAsync(key, json, options);
    }

}