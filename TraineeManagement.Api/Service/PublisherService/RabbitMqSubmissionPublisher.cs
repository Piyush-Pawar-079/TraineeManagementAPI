using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CommonLibrary.Contract;
using TraineeManagement.Api.Service.CorrelationIdService;

namespace TraineeManagement.Api.Service.PublisherService;

public class RabbitMqSubmissionPublisher : IRabbitMqPublisher
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private readonly ILogger<RabbitMqSubmissionPublisher> _logger;
    private readonly string correlationId;

    public RabbitMqSubmissionPublisher(ILogger<RabbitMqSubmissionPublisher> logger, ICorrelationIdAccessor correlationIdAccessor)
    {
        _logger = logger;
        correlationId = correlationIdAccessor.GetCorrelationId();

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

        string QueueName = Environment.GetEnvironmentVariable("RabbitMQ_QueueName") ?? "submission-processor";

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArguments);
        await _channel.QueueDeclareAsync($"{QueueName}-failed", durable: true, exclusive: false, autoDelete: false);

        await _channel.QueueBindAsync(QueueName, "submission-exchange", "requested");
        await _channel.QueueBindAsync($"{QueueName}-failed", "submission-dlx", "failed");

        _logger.LogInformation("Queue, exchange and routes created successfully. CorrelationId: {CorrelationId}", correlationId);
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
            _logger.LogError(ex, "Broker unavailable. Failed to publish message: {MessageId}. CorrelationId: {CorrelationId}", message.MessageId, message.CorrelationId);
            return false;
        }
    }

    public async Task Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
