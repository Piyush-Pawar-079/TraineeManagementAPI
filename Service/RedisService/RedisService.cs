using System.Text.Json;
using StackExchange.Redis;

namespace traineeManagementAPI.Service.RedisService;

public class RedisService(IConnectionMultiplexer redis): IRedisService
{
    private readonly IDatabase _db = redis.GetDatabase();

    public async Task<T?> GetAsync<T>(string key)
    {
        var value = await _db.StringGetAsync(key);
        if (value.IsNullOrEmpty)
        {
            return default;
        }
        return JsonSerializer.Deserialize<T>(value.ToString());
    }

    public async Task RemoveAsync(string key)
    {
        await _db.KeyDeleteAsync(key);
    }

    public async Task SetAsync<T>(string key, T value, double? expiry = null)
    {
        var json = JsonSerializer.Serialize(value);
        await _db.StringSetAsync(key, json);
    }

}