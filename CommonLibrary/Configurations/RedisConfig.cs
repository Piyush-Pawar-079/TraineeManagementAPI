
namespace CommonLibrary.Configurations;

public class RedisConfig
{
    public const string SectionName = "Redis";
    public string ConnectionString {get; set; }= string.Empty;
    public string InstanceName { get; set; } = string.Empty;
    public int DefaultItlSeconds { get; set; }
}