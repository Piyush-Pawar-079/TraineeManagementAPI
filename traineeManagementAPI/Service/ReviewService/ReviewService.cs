using AutoMapper;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.Exceptions;
using CommonLibrary.Models;
using traineeManagementAPI.Repositories.ReviewRepository;
using traineeManagementAPI.Service.CorrelationIdService;

namespace traineeManagementAPI.Service.ReviewService;

public class ReviewService(IReviewRepository repository, ILogger<ReviewService> logger, IMapper mapper, ICorrelationIdAccessor correlationIdAccessor) : IReviewService
{
    private readonly IReviewRepository _repo = repository;
    private readonly ILogger<ReviewService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();

    public async Task<List<ReviewDetailDTO>> GetAllAsync()
    {
        var allReviews = await _repo.GetAllReviewsAsync();
        return _mapper.Map<List<ReviewDetailDTO>>(allReviews);
    }

    public async Task<ReviewDetailDTO?> GetByIdAsync(int id)
    {
        var desiredReview = await _repo.GetReviewByIdAsync(id);
        if (desiredReview == null)
        {
            _logger.LogError("Review with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Review with the id - {id} not found");
        } 
        return _mapper.Map<ReviewDetailDTO>(desiredReview);

    }

    public async Task<ReviewDetailDTO> CreateAsync(CreateReviewRequestDTO createReviewDto)
    {
        // Review newReview = new()
        // {
        //     SubmissionId = createReviewDto.SubmissionId,
        //     MentorId = createReviewDto.MentorId,
        //     Feedback = createReviewDto.Feedback,
        //     Score = createReviewDto.Score,
        //     ReviewStatus = createReviewDto.ReviewStatus,
        //     ReviewedDate = createReviewDto.ReviewedDate
        // };

        Review newReview = _mapper.Map<Review>(createReviewDto);

        Review CreatedReview = await _repo.CreateReviewAsync(newReview);

        if (CreatedReview == null)
        {
            _logger.LogError("Something went wrong while creating a new Review. CorrelationId: {CorrelationId}", correlationId);
            throw new Exception("Something went wrong while creating a new Review");
        }
        return _mapper.Map<ReviewDetailDTO>(CreatedReview);

    }

}