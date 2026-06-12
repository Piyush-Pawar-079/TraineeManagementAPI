using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.SubmissionRepository;

namespace traineeManagementAPI.Service.SubmissionService;

public class SubmissionService(ISubmissionRepository repository, ILogger<SubmissionService> logger) : ISubmissionService
{
    private readonly ISubmissionRepository _repo = repository;
    private readonly ILogger<SubmissionService> _logger = logger;
    private static int _nextId = 0;

    private SubmissionResponseDTO MapToSubmissionResponseDTO(Submission Submission)
    {
        return new SubmissionResponseDTO
        {
            Id = Submission.Id,
            TaskAssignmentId = Submission.TaskAssignmentId,
            SubmissionUrl = Submission.SubmissionUrl,
            Notes = Submission.Notes,
            SubmittedDate = Submission.SubmittedDate,
            Status = Submission.Status
        };
    }

    public async Task<List<SubmissionResponseDTO>> GetAllAsync()
    {
        var allSubmissions = await _repo.GetAllSubmissionsAsync();
        return allSubmissions.Select(MapToSubmissionResponseDTO).ToList();
    }

    public async Task<SubmissionResponseDTO?> GetByIdAsync(int id)
    {
        var desiredSubmission = await _repo.GetSubmissionByIdAsync(id);

        if (desiredSubmission == null)
        {
            return null;
        }

        return MapToSubmissionResponseDTO(desiredSubmission);

    }

    public async Task<SubmissionResponseDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto)
    {
        Submission newSubmission = new()
        {
            Id = _nextId,
            TaskAssignmentId = createSubmissionDto.TaskAssignmentId,
            SubmissionUrl = createSubmissionDto.SubmissionUrl,
            Notes = createSubmissionDto.Notes,
            SubmittedDate = createSubmissionDto.SubmittedDate,
            Status = createSubmissionDto.Status
        };

        _nextId++;

        Submission CreatedSubmission = await _repo.CreateSubmissionAsync(newSubmission);

        return MapToSubmissionResponseDTO(CreatedSubmission);

    }

}