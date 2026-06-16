using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.ReviewRepository;

namespace traineeManagementAPI.Service.ReviewService;

public class ReviewService(IReviewRepository repository, ILogger<ReviewService> logger) : IReviewService
{
    private readonly IReviewRepository _repo = repository;
    private readonly ILogger<ReviewService> _logger = logger;

    public ReviewResponseDTO MapToReviewResponseDTO(Review Review)
    {
        return new ReviewResponseDTO
        {
            Id = Review.Id,
            SubmissionId = Review.SubmissionId,
            Submission = new SubmissionResponseDTO
                {
                    Id = Review.Submission.Id,
                    TaskAssignmentId = Review.Submission.TaskAssignmentId,
                    SubmissionUrl = Review.Submission.SubmissionUrl,
                    Notes = Review.Submission.Notes,
                    SubmittedDate = Review.Submission.SubmittedDate,
                    Status = Review.Submission.Status
                },
            MentorId = Review.MentorId,
            Mentor = new MentorResponseDTO
        {
            Id = Review.Mentor.Id,
            FirstName = Review.Mentor.FirstName,
            LastName = Review.Mentor.LastName,
            Email = Review.Mentor.Email,
            Expertise = Review.Mentor.Expertise,
            Status = Review.Mentor.Status,
            TaskAssignments = Review.Mentor.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
        {
            Id = ta.Id,
            TraineeId = ta.TraineeId,
            MentorId = ta.MentorId,
            LearningTaskId = ta.LearningTaskId,
            AssignedDate = ta.AssignedDate,
            DueDate = ta.DueDate,
            Status = ta.Status,
            Remarks = ta?.Remarks
        }).ToList(),
            Reviews = Review.Mentor.Reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            SubmissionId = r.SubmissionId,
            MentorId = r.MentorId,
            Feedback = r.Feedback,
            Score = r.Score ?? null,
            ReviewStatus = r.ReviewStatus,
            ReviewedDate = r.ReviewedDate
        }).ToList(),
            CreatedDate = Review.Mentor.CreatedDate,
            UpdatedDate = Review.Mentor.UpdatedDate
        },
            Feedback = Review.Feedback,
            Score = Review.Score ?? null,
            ReviewStatus = Review.ReviewStatus,
            ReviewedDate = Review.ReviewedDate
        };
    }

    public async Task<List<ReviewResponseDTO>> GetAllAsync()
    {
        var allReviews = await _repo.GetAllReviewsAsync();
        return allReviews.Select(MapToReviewResponseDTO).ToList();
    }

    public async Task<ReviewResponseDTO?> GetByIdAsync(int id)
    {
        var desiredReview = await _repo.GetReviewByIdAsync(id);
        if (desiredReview == null)
        {
            _logger.LogError("Review with the specified Id is not available.");
            throw new NotFoundException($"Review with the id - {id} not found");
        }
        return MapToReviewResponseDTO(desiredReview);

    }

    public async Task<ReviewResponseDTO> CreateAsync(CreateReviewRequestDTO createReviewDto)
    {
        Review newReview = new()
        {
            SubmissionId = createReviewDto.SubmissionId,
            MentorId = createReviewDto.MentorId,
            Feedback = createReviewDto.Feedback,
            Score = createReviewDto.Score,
            ReviewStatus = createReviewDto.ReviewStatus,
            ReviewedDate = createReviewDto.ReviewedDate
        };

        Review CreatedReview = await _repo.CreateReviewAsync(newReview);

        if (CreatedReview == null)
        {
            _logger.LogError("Something went wrong while creating a new Review.");
            throw new Exception("Something went wrong while creating a new Review");
        }
        return MapToReviewResponseDTO(CreatedReview);

    }

}