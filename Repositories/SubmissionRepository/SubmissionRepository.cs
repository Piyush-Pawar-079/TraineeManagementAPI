using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.SubmissionRepository;

public class SubmissionRepository(ApplicationDBContext context) : ISubmissionRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<Submission>> GetAllSubmissionsAsync()
    {
        return await _context.Submissions.Include(s => s.TaskAssignment).ToListAsync();
    }

    public async Task<Submission?> GetSubmissionByIdAsync(int id)
    {
        return await _context.Submissions.Include(s => s.TaskAssignment).FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<Submission> CreateSubmissionAsync(Submission submission)
    {
        _context.Submissions.Add(submission);
        await _context.SaveChangesAsync();
        return submission;
    }

}