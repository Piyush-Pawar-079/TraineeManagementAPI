using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using CommonLibrary.Contract;
using TraineeManagement.Api.Service.CorrelationIdService;
using Microsoft.Extensions.Options;
using CommonLibrary.Configurations;

namespace TraineeManagement.Api.Service.PublisherService;

public class RabbitMqSubmissionPublisher : IRabbitMqPublisher
{
    private readonly ConnectionFactory _factory;
    private IConnection _connection = null!;
    private IChannel _channel = null!;
    private readonly ILogger<RabbitMqSubmissionPublisher> _logger;
    private readonly string correlationId;
    private readonly RabbitMqConfig rabbitMqConfig;

    public RabbitMqSubmissionPublisher(ILogger<RabbitMqSubmissionPublisher> logger, ICorrelationIdAccessor correlationIdAccessor, IOptions<RabbitMqConfig> options)
    {
        _logger = logger;
        correlationId = correlationIdAccessor.GetCorrelationId();
        rabbitMqConfig = options.Value;

        _factory = new ConnectionFactory
        {
            HostName = rabbitMqConfig.HostName,
            Port = rabbitMqConfig.Port,
            VirtualHost = rabbitMqConfig.VirtualHost,
            UserName = rabbitMqConfig.UserName,
            Password = rabbitMqConfig.Password
        };
    }

    public async Task Setup()
    {
        _connection = await _factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Declare main queue and dead-letter exchange/queue relationships
        await _channel.ExchangeDeclareAsync(rabbitMqConfig.SubmissionQueueExchange, ExchangeType.Direct, durable: true);
        await _channel.ExchangeDeclareAsync(rabbitMqConfig.DlxName, ExchangeType.Direct, durable: true);

        IDictionary<string, object?> queueArguments = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", rabbitMqConfig.DlxName },
            { "x-dead-letter-routing-key", rabbitMqConfig.RoutingKeyDlx }
        };

        string QueueName = rabbitMqConfig.SubmissionQueue;

        await _channel.QueueDeclareAsync(QueueName, durable: true, exclusive: false, autoDelete: false, arguments: queueArguments);
        await _channel.QueueDeclareAsync($"{QueueName}-failed", durable: true, exclusive: false, autoDelete: false);

        await _channel.QueueBindAsync(QueueName, rabbitMqConfig.SubmissionQueueExchange, rabbitMqConfig.SubmissionRoutingKey);
        await _channel.QueueBindAsync($"{QueueName}-failed", rabbitMqConfig.DlxName, rabbitMqConfig.SubmissionRoutingKey);

        _logger.LogInformation("Queue, exchange and routes created successfully. CorrelationId: {CorrelationId}", correlationId);
    }

    public async Task<bool> PublishSubmissionRequestedAsync(SubmissionProcessingRequested message)
    {
        try
        {

            if (_channel == null)
            {
                await Setup();
            }

            var json = JsonSerializer.Serialize(message);
            var body = Encoding.UTF8.GetBytes(json);

            var properties = new BasicProperties
            {
                Persistent = true, // Task 3.9 Survival requirement
                MessageId = message.MessageId.ToString(),
                CorrelationId = message.CorrelationId.ToString()
            };

            await _channel.BasicPublishAsync(
                exchange: rabbitMqConfig.SubmissionQueueExchange,
                routingKey: rabbitMqConfig.SubmissionRoutingKey,
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
            _logger.LogDebug(ex, "Broker unavailable. Failed to publish message: {MessageId}. CorrelationId: {CorrelationId}", message.MessageId, message.CorrelationId);
            return false;
        }
    }

    public async Task Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
