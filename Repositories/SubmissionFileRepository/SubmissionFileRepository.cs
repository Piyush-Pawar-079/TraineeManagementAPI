using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.SubmissionFileRepository;

public class SubmissionFileRepository(ApplicationDBContext context) : ISubmissionFileRepository
{
    private readonly ApplicationDBContext _context = context;
    
    public async Task<SubmissionFile> SaveAsync(SubmissionFile submissionFile)
    {
        await _context.SubmissionFiles.AddAsync(submissionFile);
        await _context.SaveChangesAsync();
        return submissionFile;
    }

    public async Task<SubmissionFile?> GetSubmissionFileById(int id)
    {
        return await _context.SubmissionFiles.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task DeleteAsync(SubmissionFile submissionFile)
    {
        _context.SubmissionFiles.Remove(submissionFile);
        await _context.SaveChangesAsync();
    }

}