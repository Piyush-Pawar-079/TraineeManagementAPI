using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.SubmissionRepository;

namespace traineeManagementAPI.Service.SubmissionService;

public class SubmissionService(ISubmissionRepository repository, ILogger<SubmissionService> logger) : ISubmissionService
{
    private readonly ISubmissionRepository _repo = repository;
    private readonly ILogger<SubmissionService> _logger = logger;

    public SubmissionResponseDTO MapToSubmissionResponseDTO(Submission Submission)
    {
        return new SubmissionResponseDTO
        {
            Id = Submission.Id,
            TaskAssignmentId = Submission.TaskAssignmentId,
            TaskAssignment = new TaskAssignmentResponseDTO
                {
                    Id = Submission.TaskAssignment.Id,
                    TraineeId = Submission.TaskAssignment.TraineeId,
                    MentorId = Submission.TaskAssignment.MentorId,
                    LearningTaskId = Submission.TaskAssignment.LearningTaskId,
                    AssignedDate = Submission.TaskAssignment.AssignedDate,
                    DueDate = Submission.TaskAssignment.DueDate,
                    Status = Submission.TaskAssignment.Status,
                    Remarks = Submission.TaskAssignment?.Remarks
                },
            Reviews = Submission.Reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            SubmissionId = r.SubmissionId,
            MentorId = r.MentorId,
            Feedback = r.Feedback,
            Score = r.Score ?? null,
            ReviewStatus = r.ReviewStatus,
            ReviewedDate = r.ReviewedDate
        }).ToList(),
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
            _logger.LogError("Submission with the specified Id is not available.");
            throw new NotFoundException($"Submission with the id - {id} not found");
        }

        return MapToSubmissionResponseDTO(desiredSubmission);

    }

    public async Task<SubmissionResponseDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto)
    {
        Submission newSubmission = new()
        {
            TaskAssignmentId = createSubmissionDto.TaskAssignmentId,
            SubmissionUrl = createSubmissionDto.SubmissionUrl,
            Notes = createSubmissionDto.Notes,
            SubmittedDate = createSubmissionDto.SubmittedDate,
            Status = createSubmissionDto.Status
        };

        Submission CreatedSubmission = await _repo.CreateSubmissionAsync(newSubmission);

        if (CreatedSubmission == null)
        {
            _logger.LogError("Something went wrong while creating a new Submission.");
            throw new Exception("Something went wrong while creating a new Submission");
        }

        return MapToSubmissionResponseDTO(CreatedSubmission);

    }

}