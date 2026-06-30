using AutoMapper;
using CommonLibrary.Contract;
using CommonLibrary.Models;
using TraineeManagement.Api.DTO.ProcessingJobDTOs;
using TraineeManagement.Api.Exceptions;
using TraineeManagement.Api.Repositories.ProcessingJobRepository;
using TraineeManagement.Api.Service.CorrelationIdService;
using TraineeManagement.Api.Service.PublisherService;

namespace TraineeManagement.Api.Service.ProcessingJobService;

public class ProcessingJobService(IProcessingJobRepository repo, ILogger<ProcessingJobService> logger, IMapper mapper, ICorrelationIdAccessor correlationIdAccessor, IRabbitMqPublisher publisher) : IProcessingJobService
{
    private readonly IProcessingJobRepository _repo = repo;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ProcessingJobService> _logger = logger;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();
    private readonly IRabbitMqPublisher _publisher = publisher;

    public async Task<ProcessingJobResponseDTO> GetProcessingJobById(int id)
    {
        var job = await _repo.GetJobById(id);

        if (job == null)
        {
            _logger.LogError($"Processing Job with the specified Id - {id} is not available. CorrelationId: {correlationId}");
            throw new NotFoundException($"Processing Job with the specified Id - {id} is not available");
        }

        return _mapper.Map<ProcessingJobResponseDTO>(job);
    }

    public async Task AddProcessingJob(ProcessingJob job)
    {
        await _repo.AddJobAsync(job);
    }

    public async Task<ProcessingJobResponseDTO?> RetryJobAsync(int id)
    {
        var job = await GetProcessingJobById(id);

        if (job == null)
        {
            return null;
        }

        job.Status = JobStatus.Queued;

        var message = new SubmissionProcessingRequested
        {
            CorrelationId = correlationId,
            SubmissionId = job.SubmissionId,
            FileId = job.FileId.ToString(),
            RequestedAt = DateTime.UtcNow
        };

        bool isQueued = await _publisher.PublishSubmissionRequestedAsync(message);

        if (!isQueued)
        {
            _logger.LogInformation(503, "Database updated, but processing queue is currently unavailable. Retry later. CorrelationId: {CorrelationId}", correlationId);
        }
        else
            _logger.LogInformation("Message added to the processing queue. CorrelationId: {CorrelationId}", correlationId);

        return _mapper.Map<ProcessingJobResponseDTO>(job);
    }

}