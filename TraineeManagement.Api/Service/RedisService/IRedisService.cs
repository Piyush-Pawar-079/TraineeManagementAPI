namespace TraineeManagement.Api.Service.RedisService;

public interface IRedisService
{
    Task SetAsync<T>(string key, T value, double? expiry = null);

    Task<T?> GetAsync<T>(string key);

    Task RemoveAsync(string key);

}