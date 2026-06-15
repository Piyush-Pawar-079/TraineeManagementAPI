using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.ReviewRepository;

public class ReviewRepository(ApplicationDBContext context) : IReviewRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<Review>> GetAllReviewsAsync()
    {
        return await _context.Reviews.Include(s => s.Submission).Include(s => s.Mentor).ThenInclude(m => m.TaskAssignments).ToListAsync();
    }

    public async Task<Review?> GetReviewByIdAsync(int id)
    {
        return await _context.Reviews.Include(s => s.Submission).Include(s => s.Mentor).ThenInclude(m => m.TaskAssignments).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Review> CreateReviewAsync(Review Review)
    {
        _context.Reviews.Add(Review);
        await _context.SaveChangesAsync();
        return Review;
    }

}