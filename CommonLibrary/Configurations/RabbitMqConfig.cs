namespace CommonLibrary.Configurations;

public class RabbitMqConfig
{
    public const string SectionName = "RabbitMq";
    public string HostName {get; set; } = string.Empty;
    public int Port {get; set;}
    public string VHost {get; set;} = string.Empty;
    public string  UserName {get; set;} = string.Empty;
    public string Password {get; set;} = string.Empty;
    public string SubmissionQueue {get; set;} = string.Empty;

    public string SubmissionQueueExchange { get; set;} = string.Empty;
    public string DlxName {get; set;} = string.Empty;

    public string DlqName {get; set;} = string.Empty;

    public string RoutingKeyDlx {get; set;} = string.Empty;

    public string SubmissionRoutingKey {get; set;} = string.Empty;

    public int MaxRetryAttempts {get; set;} = 3;

}