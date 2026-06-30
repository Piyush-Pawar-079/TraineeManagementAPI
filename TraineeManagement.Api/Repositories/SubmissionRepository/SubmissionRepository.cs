using Microsoft.EntityFrameworkCore;
using CommonLibrary.Models;
using CommonLibrary.Data;

namespace TraineeManagement.Api.Repositories.SubmissionRepository;

public class SubmissionRepository(ApplicationDBContext context) : ISubmissionRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await _context.Submissions.Include(s => s.TaskAssignment).Include(s => s.Reviews).ToListAsync();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(int id)
    {
        return await _context.Submissions.Include(s => s.TaskAssignment).Include(s => s.Reviews).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Submission> CreateSubmissionAsync(Submission submission)
    {
        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

}