using System.Text;
using System.Text.Json;
using CommonLibrary.Contract;
using CommonLibrary.Data;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;


namespace SubmissionProcessor.Worker;

public class SubmissionProcessorWorker(
    IServiceProvider serviceProvider,
    ILogger<SubmissionProcessorWorker> logger) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<SubmissionProcessorWorker> _logger = logger;
    private IConnection? _connection;
    private IChannel? _channel;

    private async Task InitializeRabbitMq()
    {
        var factory = new ConnectionFactory
        {
            HostName = Environment.GetEnvironmentVariable("RabbitMQ_Host")!,
            Port = int.Parse(Environment.GetEnvironmentVariable("RabbitMQ_Port")!),
            VirtualHost = Environment.GetEnvironmentVariable("RabbitMQ_VHost")!,
            UserName = Environment.GetEnvironmentVariable("RabbitMQ_UserName")!,
            Password = Environment.GetEnvironmentVariable("RabbitMQ_Password")!
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Limit data chunking processing window
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);
    }

    protected async override Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {

        if (_channel == null)
        {
            await InitializeRabbitMq();
        }

        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            Console.WriteLine("Inside the received async thing : " + ea);
            await HandleMessageDeliveryAsync(ea);
        };
        await _channel.BasicConsumeAsync(queue: Environment.GetEnvironmentVariable("RabbitMQ_QueueName")!, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);

        _logger.LogError("Something is done by the worker.");
        return Task.CompletedTask;
    }

    private async Task HandleMessageDeliveryAsync(BasicDeliverEventArgs ea)
    {
        using var scope = _serviceProvider.CreateScope();
        Console.WriteLine("Literaly inside that function: " + scope);
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        SubmissionProcessingRequested? message = null;

        try
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            message = JsonSerializer.Deserialize<SubmissionProcessingRequested>(json);
            Console.WriteLine("Body and json: " + body + json);
            if (message == null)
            {
                _logger.LogError("Corrupted data structural package payload. Dropping.");
                await _channel.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                return;
            }

            // Task 3.15 Idempotency Guard
            var alreadyProcessed = await dbContext.ProcessedMessages.AnyAsync(m => m.MessageId == message.MessageId);
            if (alreadyProcessed)
            {
                _logger.LogWarning("Duplicate delivery detected for message ID: {MessageId}. Skipping processing.", message.MessageId);
                await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                return;
            }

            // Fetch state and track processing attempt lifecycle
            var job = await dbContext.ProcessingJobs.FirstOrDefaultAsync(j => j.CorrelationId == message.CorrelationId);
            if (job != null)
            {
                if (job.Status == JobStatus.Completed)
                {
                    _logger.LogWarning("Job already marked complete for Correlation ID: {CorrelationId}.", message.CorrelationId);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }

                job.Status = JobStatus.Processing;
                job.Attempts++;
                job.StartedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
            }

            // Task 3.13 Executing safe structural computational metadata calculations
            await ProcessBusinessLogicSimulationAsync(message);

            // Save processing updates state securely
            if (job != null)
            {
                job.Status = JobStatus.Completed;
                job.CompletedAt = DateTime.UtcNow;
            }

            dbContext.ProcessedMessages.Add(new ProcessedMessage { MessageId = message.MessageId });
            await dbContext.SaveChangesAsync();

            // Task 3.12 Confirm broker clearing validation
            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution exception thrown during processing of payload package.");
            await HandleFailureLifecycleAsync(ea, dbContext, message, ex);
        }
    }

    private async Task ProcessBusinessLogicSimulationAsync(SubmissionProcessingRequested msg)
    {
        // Simulate IO computation operations safely
        await Task.Delay(1500);

        if (string.IsNullOrEmpty(msg.FileId))
        {
            throw new InvalidOperationException("Fatal business structural validation error: File data target path key cannot be null.");
        }
        
        // Simulating transient failure logic for validation verification
        if (msg.ContractVersion == "fail-transient")
        {
            throw new TimeoutException("Database communication link timed out.");
        }
    }

    private async Task HandleFailureLifecycleAsync(BasicDeliverEventArgs ea, ApplicationDBContext context, SubmissionProcessingRequested? msg, Exception ex)
    {
        if (msg == null || _channel == null) return;

        try
        {
            var job = await context.ProcessingJobs.FirstOrDefaultAsync(j => j.CorrelationId == msg.CorrelationId);
            bool isTransient = ex is TimeoutException || ex is HttpRequestException;

            if (job != null)
            {
                job.ErrorSummary = ex.Message;
                
                // Task 3.16 Evaluation limits condition checking logic
                if (job.Attempts >= 3 || !isTransient)
                {
                    job.Status = JobStatus.Failed;
                    job.CompletedAt = DateTime.UtcNow;
                    await context.SaveChangesAsync();

                    _logger.LogCritical("Permanent error reached for Job {JobId}. Rejecting to DLX without requeue.", job.Id);
                    await _channel.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                }
                else
                {
                    await context.SaveChangesAsync();
                    _logger.LogWarning("Transient issue handling loop counter matched. Requeuing message strategy.");
                    await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
                }
            }
        }
        catch (Exception dbEx)
        {
            _logger.LogError(dbEx, "Failed to persist structural framework infrastructure log errors.");
            await _channel.BasicNackAsync(ea.DeliveryTag, multiple: false, requeue: true);
        }
    }

    public async Task Dispose()
    {
        _channel?.CloseAsync();
        _connection?.CloseAsync();
    }
}
