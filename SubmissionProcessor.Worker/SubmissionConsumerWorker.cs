using System.Text;
using System.Text.Json;
using CommonLibrary.Contract;
using CommonLibrary.Data;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SubmissionProcessor.Worker.Clients;


namespace SubmissionProcessor.Worker;

public class SubmissionProcessorWorker(
    IServiceProvider serviceProvider,
    ILogger<SubmissionProcessorWorker> logger,
    HttpDirectoryClient client) : BackgroundService
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;
    private readonly ILogger<SubmissionProcessorWorker> _logger = logger;
    private readonly HttpDirectoryClient _client = client;
    private IConnection _connection = null!;
    private IChannel _channel = null!;

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

        _logger.LogInformation("Connecting to RabbitMQ...");

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        var queueName = Environment.GetEnvironmentVariable("RabbitMQ_QueueName") ?? "submission-processor";

        await _channel.ExchangeDeclareAsync("submission-exchange", ExchangeType.Direct, durable: true);
        await _channel.ExchangeDeclareAsync("submission-dlx", ExchangeType.Direct, durable: true);

        IDictionary<string, object?> queueArguments = new Dictionary<string, object?>
        {
            { "x-dead-letter-exchange", "submission-dlx" },
            { "x-dead-letter-routing-key", "failed" }
        };

        await _channel.QueueDeclareAsync(
            queue: queueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: queueArguments
        );

        await _channel.QueueDeclareAsync(
            queue: $"{queueName}-failed",
            durable: true,
            exclusive: false,
            autoDelete: false
        );

        await _channel.QueueBindAsync(queueName, "submission-exchange", "requested");
        await _channel.QueueBindAsync($"{queueName}-failed", "submission-dlx", "failed");

        await _channel.BasicQosAsync(0, 1, false);

        _logger.LogInformation("RabbitMQ setup complete. Queue: {Queue}", queueName);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Worker started...");

        try
        {
            var trainee = await _client.GetTraineeByIdAsync(1, stoppingToken);

            if (trainee != null)
            {
                _logger.LogInformation(
                    "Trainee: {Name}, Course: {Course}, Completed: {CompletedAssignments}",
                    trainee.Name,
                    trainee.Course,
                    trainee.CompletedAssignments);
            }
            else
            {
                _logger.LogWarning("Could not fetch trainee data");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "API call failed");
        }
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await InitializeRabbitMq();
                break;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "RabbitMQ not ready. Retrying in 5 seconds...");
                await Task.Delay(10000, stoppingToken);
            }
        }

        var queueName = Environment.GetEnvironmentVariable("RabbitMQ_QueueName")!;

        var consumer = new AsyncEventingBasicConsumer(_channel);

        consumer.ReceivedAsync += async (model, ea) =>
        {
            await HandleMessageDeliveryAsync(ea);
        };

        await _channel.BasicConsumeAsync(
            queue: queueName,
            autoAck: false,
            consumer: consumer,
            cancellationToken: stoppingToken
        );

        _logger.LogInformation("Worker is now listening to queue: {Queue}", queueName);

        await Task.Delay(Timeout.Infinite, stoppingToken);
    }


    private async Task HandleMessageDeliveryAsync(BasicDeliverEventArgs ea)
    {
        using var scope = _serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
        SubmissionProcessingRequested? message = null;

        try
        {
            var body = ea.Body.ToArray();
            var json = Encoding.UTF8.GetString(body);
            message = JsonSerializer.Deserialize<SubmissionProcessingRequested>(json);
            if (message == null)
            {
                _logger.LogError("Corrupted data structural package payload. Dropping.");
                await _channel.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                return;
            }

            // Task 3.15 Idempotency Guard
            // Fetch state and track processing attempt lifecycle
            var job = await dbContext.ProcessingJobs.FirstOrDefaultAsync(j => j.CorrelationId == message.CorrelationId);
            if (job != null)
            {
                _logger.LogInformation("Job is being processed. CorrelationId: {CorrelationId}", message.CorrelationId);
                if (job.Status == JobStatus.Completed)
                {
                    _logger.LogWarning("Job already marked complete for Correlation ID: {CorrelationId}.", message.CorrelationId);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }

                if (job.Attempts > 3)
                {
                    _logger.LogWarning("Job processing attempts exceeds 3. Job failed. Correlation ID: {CorrelationId}", message.CorrelationId);
                    job.Status = JobStatus.Failed;
                    job.CompletedAt = DateTime.UtcNow;
                    await dbContext.SaveChangesAsync();
                    await _channel.BasicRejectAsync(ea.DeliveryTag, requeue: false);
                    return;
                }

                job.Status = JobStatus.Processing;
                job.Attempts++;
                job.StartedAt = DateTime.UtcNow;
                await dbContext.SaveChangesAsync();
                
                // Task 3.13 Executing safe structural computational metadata calculations
                await ProcessBusinessLogicSimulationAsync(message, dbContext, job);
            }

            // Task 3.12 Confirm broker clearing validation
            await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Execution exception thrown during processing of payload package.");
        }
    }

    private async Task ProcessBusinessLogicSimulationAsync(SubmissionProcessingRequested msg, ApplicationDBContext dbcontext, ProcessingJob job)
    {

        _logger.LogInformation("Job being processed.  CorrelationId: {CorrelationId}", msg.CorrelationId);

        if (string.IsNullOrEmpty(msg.FileId))
        {
            _logger.LogInformation("Job cannot be processed. File id not available.  CorrelationId: {CorrelationId}", msg.CorrelationId);
            throw new InvalidOperationException("Fatal business structural validation error: File data target path key cannot be null.");
        }
        
        // Simulating transient failure logic for validation verification
        if (msg.ContractVersion == "fail-transient")
        {
            _logger.LogInformation("Job cannot be processed. Transient failure occurred.  CorrelationId: {CorrelationId}", msg.CorrelationId);
            throw new TimeoutException("Database communication link timed out.");
        }

        var submissionFile = await dbcontext.SubmissionFiles.FirstOrDefaultAsync(sf => sf.Id == int.Parse(msg.FileId));

        if (submissionFile == null)
        {
            // Save processing updates state securely
            if (job != null)
            {
                job.Status = JobStatus.Failed;
                job.CompletedAt = DateTime.UtcNow;
                await dbcontext.SaveChangesAsync();
            }
            _logger.LogError($"Submission File with the file Id - {msg.FileId} is not present in the database.  CorrelationId: {msg.CorrelationId}");
            throw new FileNotFoundException($"Submission File with the file Id - {msg.FileId} is not present in the database");
        }
        // Save processing updates state securely
        if (job != null)
        {
            job.Status = JobStatus.Completed;
            job.CompletedAt = DateTime.UtcNow;
            await dbcontext.SaveChangesAsync();
            _logger.LogError($"Job processed successfully. CorrelationId: {msg.CorrelationId}");
        }
        return;

    }
}
