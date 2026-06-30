using AutoMapper;
using CommonLibrary.Models;
using TraineeManagement.Api.DTO.ProcessingJobDTOs;
using TraineeManagement.Api.Exceptions;
using TraineeManagement.Api.Repositories.ProcessingJobRepository;
using TraineeManagement.Api.Service.CorrelationIdService;

namespace TraineeManagement.Api.Service.ProcessingJobService;

public class ProcessingJobService(IProcessingJobRepository repo, ILogger<ProcessingJobService> logger, IMapper mapper, ICorrelationIdAccessor correlationIdAccessor) : IProcessingJobService
{
    private readonly IProcessingJobRepository _repo = repo;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ProcessingJobService> _logger = logger;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();

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

}