using AutoMapper;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.SubmissionRepository;

namespace traineeManagementAPI.Service.SubmissionService;

public class SubmissionService(ISubmissionRepository repository, ILogger<SubmissionService> logger, IMapper mapper) : ISubmissionService
{
    private readonly ISubmissionRepository _repo = repository;
    private readonly ILogger<SubmissionService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<List<SubmissionDetailDTO>> GetAllAsync()
    {
        var allSubmissions = await _repo.GetAllSubmissionsAsync();
        return _mapper.Map<List<SubmissionDetailDTO>>(allSubmissions);
    }

    public async Task<SubmissionDetailDTO?> GetByIdAsync(int id)
    {
        var desiredSubmission = await _repo.GetSubmissionByIdAsync(id);

        if (desiredSubmission == null)
        {
            _logger.LogError("Submission with the specified Id is not available.");
            throw new NotFoundException($"Submission with the id - {id} not found");
        }

        return _mapper.Map<SubmissionDetailDTO>(desiredSubmission);

    }

    public async Task<SubmissionDetailDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto)
    {
        // Submission newSubmission = new()
        // {
        //     TaskAssignmentId = createSubmissionDto.TaskAssignmentId,
        //     SubmissionUrl = createSubmissionDto.SubmissionUrl,
        //     Notes = createSubmissionDto.Notes,
        //     SubmittedDate = createSubmissionDto.SubmittedDate,
        //     Status = createSubmissionDto.Status
        // };

        Submission newSubmission = _mapper.Map<Submission>(createSubmissionDto);

        Submission CreatedSubmission = await _repo.CreateSubmissionAsync(newSubmission);

        if (CreatedSubmission == null)
        {
            _logger.LogError("Something went wrong while creating a new Submission.");
            throw new Exception("Something went wrong while creating a new Submission");
        }

        return _mapper.Map<SubmissionDetailDTO>(CreatedSubmission);

    }

}