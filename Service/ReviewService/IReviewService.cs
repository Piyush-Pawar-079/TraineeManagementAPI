using traineeManagementAPI.DTO.ReviewDTOs;

namespace traineeManagementAPI.Service.ReviewService;

public interface IReviewService
{
    Task<List<ReviewResponseDTO>> GetAllAsync();

    Task<ReviewResponseDTO?> GetByIdAsync(int id);

    Task<ReviewResponseDTO> CreateAsync(CreateReviewRequestDTO createReviewDto);
}