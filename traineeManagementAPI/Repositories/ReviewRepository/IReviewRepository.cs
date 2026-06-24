using CommonLibrary.Models;

namespace traineeManagementAPI.Repositories.ReviewRepository;

public interface IReviewRepository
{
    Task<List<Review>> GetAllReviewsAsync();

    Task<Review?> GetReviewByIdAsync(int id);

    Task<Review> CreateReviewAsync(Review Review);
}