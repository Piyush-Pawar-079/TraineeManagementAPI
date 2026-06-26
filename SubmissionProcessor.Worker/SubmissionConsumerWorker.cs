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

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        // Limit data chunking processing window
        await _channel.BasicQosAsync(prefetchSize: 0, prefetchCount: 1, global: false);

    }

    protected async override Task<Task> ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Calling TrainingDirectory API...");

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

        await Task.Delay(5000, stoppingToken);


        if (_channel == null || _connection == null || !_connection.IsOpen)
        {
            await InitializeRabbitMq();
        }

        stoppingToken.ThrowIfCancellationRequested();

        var consumer = new AsyncEventingBasicConsumer(_channel!);
        consumer.ReceivedAsync += async (model, ea) =>
        {
            await HandleMessageDeliveryAsync(ea);
        };
        await _channel!.BasicConsumeAsync(queue: Environment.GetEnvironmentVariable("RabbitMQ_QueueName")!, autoAck: false, consumer: consumer, cancellationToken: stoppingToken);
        return Task.CompletedTask;
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
                if (job.Status == JobStatus.Completed)
                {
                    _logger.LogWarning("Job already marked complete for Correlation ID: {CorrelationId}.", message.CorrelationId);
                    await _channel.BasicAckAsync(ea.DeliveryTag, multiple: false);
                    return;
                }

                if (job.Attempts > 3)
                {
                    _logger.LogWarning("Job processing attempts exceeds 3. Correlation ID: {CorrelationId}", message.CorrelationId);
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
