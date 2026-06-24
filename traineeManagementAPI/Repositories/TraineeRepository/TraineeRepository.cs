using Microsoft.EntityFrameworkCore;
using CommonLibrary.Data;
using traineeManagementAPI.Helpers;
using CommonLibrary.Models;

namespace traineeManagementAPI.Repositories.TraineeRepository;

public class TraineeRepository(ApplicationDBContext context) : ITraineeRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<Trainee>> GetAllAsync()
    {
        return await _context.Trainees.Include(t => t.TaskAssignments)
                                        .ThenInclude(t => t.Submission)
                                        .ThenInclude(s => s.Reviews)
                                        .ToListAsync();
    }

    public async Task<Trainee?> GetByIdAsync(int id)
    {
        return await _context.Trainees.Include(t => t.TaskAssignments)
                                        .ThenInclude(t => t.Submission)
                                        .ThenInclude(s => s.Reviews)
                                        .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Trainee?> UpdateAsync(int id, Trainee trainee)
    {
        _context.Trainees.Update(trainee);
        await _context.SaveChangesAsync();
        return await _context.Trainees.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<Trainee> CreateAsync(Trainee trainee)
    {
        await _context.Trainees.AddAsync(trainee);
        await _context.SaveChangesAsync();
        return trainee;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var desiredTrainee = await _context.Trainees.FindAsync(id);
        if (desiredTrainee == null)
        {
            return false;
        }
        _context.Trainees.Remove(desiredTrainee);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<PagedResponse<Trainee>> PaginatedResponse(PaginationParams paginationParams, List<Trainee> trainees)
    {
        // var query = _context.Trainees.AsQueryable();
        // var query = trainees.AsQueryable();
        var query = trainees;
        var totalRecords = query.Count;
        var items = query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                .Take(paginationParams.PageSize)
                                .ToList();
 
        var pagedResponse = new PagedResponse<Trainee>(items, paginationParams.PageNumber, paginationParams.PageSize, totalRecords);
 
        return pagedResponse;
    }

}