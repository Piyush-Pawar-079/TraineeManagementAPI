using traineeManagementAPI.DTO.ReviewDTOs;

namespace traineeManagementAPI.Service.ReviewService;

public interface IReviewService
{
    Task<List<ReviewDetailDTO>> GetAllAsync();

    Task<ReviewDetailDTO?> GetByIdAsync(int id);

    Task<ReviewDetailDTO> CreateAsync(CreateReviewRequestDTO createReviewDto);
}