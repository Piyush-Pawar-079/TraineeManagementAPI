using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.ReviewRepository;

namespace traineeManagementAPI.Service.ReviewService;

public class ReviewService(IReviewRepository repository, ILogger<ReviewService> logger) : IReviewService
{
    private readonly IReviewRepository _repo = repository;
    private readonly ILogger<ReviewService> _logger = logger;
    private static int _nextId = 0;

    private ReviewResponseDTO MapToReviewResponseDTO(Review Review)
    {
        return new ReviewResponseDTO
        {
            Id = Review.Id,
            SubmissionId = Review.SubmissionId,
            MentorId = Review.MentorId,
            Feedback = Review.Feedback,
            Score = Review.Score,
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
            return null;
        }

        return MapToReviewResponseDTO(desiredReview);

    }

    public async Task<ReviewResponseDTO> CreateAsync(CreateReviewRequestDTO createReviewDto)
    {
        Review newReview = new()
        {
            Id = _nextId,
            SubmissionId = createReviewDto.SubmissionId,
            MentorId = createReviewDto.MentorId,
            Feedback = createReviewDto.Feedback,
            Score = createReviewDto.Score,
            ReviewStatus = createReviewDto.ReviewStatus,
            ReviewedDate = createReviewDto.ReviewedDate
        };

        _nextId++;

        Review CreatedReview = await _repo.CreateReviewAsync(newReview);

        return MapToReviewResponseDTO(CreatedReview);

    }

}