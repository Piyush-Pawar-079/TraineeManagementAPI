using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CommonLibrary.Contract;

namespace traineeManagementAPI.Service.PublisherService;
public class RabbitMqSubmissionPublisher : IRabbitMqPublisher
{
    private readonly ConnectionFactory _factory;

    // public RabbitMqSubmissionPublisher(IConfiguration config, ILogger<RabbitMqSubmissionPublisher> logger)
    // {
    //     _logger = logger;
    //     _factory = new ConnectionFactory
    //     {
    //         HostName = Environment.GetEnvironmentVariable("RabbitMQ_Host")!,
    //         Port = int.Parse(Environment.GetEnvironmentVariable("RabbitMQ_Port")!),
    //         VirtualHost = Environment.GetEnvironmentVariable("RabbitMQ_VHost")!,
    //         UserName = Environment.GetEnvironmentVariable("RabbitMQ_UserName")!,
    //         Password = Environment.GetEnvironmentVariable("RabbitMQ_Password")!
    //     };
    // }

    // public async Task<bool> PublishSubmissionRequestedAsync(SubmissionProcessingRequested message)
    // {
    //     try
    //     {
    //         using var connection = await _factory.CreateConnectionAsync();
    //         using var channel = await connection.CreateChannelAsync();

    //         // Task 3.9: Define a durable queue (Survives broker restart)
    //         await channel.QueueDeclareAsync(
    //             queue: QueueName,
    //             durable: true, 
    //             exclusive: false,
    //             autoDelete: false,
    //             arguments: null
    //         );

    //         var json = JsonSerializer.Serialize(message);
    //         var body = Encoding.UTF8.GetBytes(json);

    //         // Task 3.9: Set delivery mode to Persistent (Survives broker restart)
    //         var properties = new BasicProperties
    //         {
    //             DeliveryMode = DeliveryModes.Persistent
    //         };

    //         await channel.BasicPublishAsync(
    //             exchange: string.Empty,
    //             routingKey: QueueName,
    //             mandatory: true,
    //             basicProperties: properties,
    //             body: body
    //         );

    //         // Task 3.11: Log required tracking variables
    //         _logger.LogInformation("Successfully published message. MessageId: {MsgId}, CorrelationId: {CorrId}, SubmissionId: {SubId}", 
    //             message.MessageId, message.CorrelationId, message.SubmissionId);

    //         return true;
    //     }
    //     catch (Exception ex)
    //     {
    //         // Task 3.11: Handle broker unavailability explicitly
    //         _logger.LogError(ex, "Failed to publish message to RabbitMQ due to broker unavailability.");
    //         return false;
    //     }
    // }

    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private readonly ILogger<RabbitMqSubmissionPublisher> _logger;

    public RabbitMqSubmissionPublisher(ILogger<RabbitMqSubmissionPublisher> logger)
    {
        _logger = logger;

        _factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RabbitMQ_Host")!,
            Port = int.Parse(Environment.GetEnvironmentVariable("RabbitMQ_Port")!),
            VirtualHost = Environment.GetEnvironmentVariable("RabbitMQ_VHost")!,
            UserName = Environment.GetEnvironmentVariable("RabbitMQ_UserName")!,
            Password = Environment.GetEnvironmentVariable("RabbitMQ_Password")!
        };
    }

    public async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Declare main queue and dead-letter exchange/queue relationships
        await _channel.ExchangeDeclareAsync("submission-exchange", ExchangeType.Direct, durable: true);
        await _channel.ExchangeDeclareAsync("submission-dlx", ExchangeType.Direct, durable: true);

        IDictionary<string, object?> queueArguments = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "submission-dlx" },
            { "x-dead-letter-routing-key", "failed" }
        };

        string QueueName = Environment.GetEnvironmentVariable("RabbitMQ_QueueName") ?? "submission-processing";

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArguments);
        await _channel.QueueDeclareAsync($"{QueueName}-failed", durable: true, exclusive: false, autoDelete: false);

        await _channel.QueueBindAsync(QueueName, "submission-exchange", "requested");
        await _channel.QueueBindAsync($"{QueueName}-failed", "submission-dlx", "failed");
    }

    public async Task<bool> PublishSubmissionRequestedAsync(SubmissionProcessingRequested message)
    {
        try
        {

            await Setup();

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true, // Task 3.9 Survival requirement
                MessageId = message.MessageId.ToString(),
                CorrelationId = message.CorrelationId.ToString()
            };

            await _channel.BasicPublishAsync(
                exchange: "submission-exchange",
                routingKey: "requested",
                mandatory: true,
                basicProperties: properties,
                body: body
            );

            _logger.LogInformation(
                "Successfully published message. MessageId: {MessageId}, CorrelationId: {CorrelationId}, SubmissionId: {SubmissionId}",
                message.MessageId, message.CorrelationId, message.SubmissionId
            );
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Broker unavailable. Failed to publish message: {MessageId}", message.MessageId);
            return false;
        }
    }

    public async Task Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
