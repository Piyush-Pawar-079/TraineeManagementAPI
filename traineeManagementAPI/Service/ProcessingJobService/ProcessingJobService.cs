using AutoMapper;
using traineeManagementAPI.DTO.ProcessingJobDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Repositories.ProcessingJobRepository;

namespace traineeManagementAPI.Service.ProcessingJobService;

public class ProcessingJobService(IProcessingJobRepository repo, ILogger<ProcessingJobService> logger, IMapper mapper): IProcessingJobService
{
    private readonly IProcessingJobRepository _repo = repo;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<ProcessingJobService> _logger = logger;

    public async Task<ProcessingJobResponseDTO> GetProcessingJobById(int id)
    {
        var job = await _repo.GetJobById(id);

        if (job == null)
        {
            _logger.LogError($"Processing Job with the specified Id - {id} is not available");
            throw new NotFoundException($"Processing Job with the specified Id - {id} is not available");
        }

        return _mapper.Map<ProcessingJobResponseDTO>(job);

    }

}