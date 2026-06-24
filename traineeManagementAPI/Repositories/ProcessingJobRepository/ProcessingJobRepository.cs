using CommonLibrary.Data;
using CommonLibrary.Models;
using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.DTO.ProcessingJobDTOs;

namespace traineeManagementAPI.Repositories.ProcessingJobRepository;

public class ProcessingJobRepository(ApplicationDBContext context): IProcessingJobRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<ProcessingJob?> GetJobById(int id)
    {
        // Reports authoritative database state rather than transient broker states
        return await _context.ProcessingJobs.FirstOrDefaultAsync(j => j.Id == id);
    }

    public async Task AddJobAsync(ProcessingJob job)
    {
        _context.ProcessingJobs.Add(job);
        await _context.SaveChangesAsync();
    }

}