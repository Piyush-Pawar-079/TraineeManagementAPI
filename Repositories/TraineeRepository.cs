

using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Helpers;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories;

public class TraineeRepository : ITraineeRepository
{
    private readonly ApplicationDBContext _context;

    public TraineeRepository(ApplicationDBContext context)
    {
        _context = context;
    }

    public async Task<List<Trainee>> GetAllAsync()
    {
        return await _context.Trainees.ToListAsync();
    }

    public async Task<Trainee?> GetByIdAsync(int id)
    {
        return await _context.Trainees.FirstOrDefaultAsync(t => t.Id == id);
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

    public async Task<PagedResponse<Trainee>> PaginatedResponse(PaginationParams paginationParams)
    {
        var query = _context.Trainees.AsQueryable();
        var totalRecords = await query.CountAsync();
        var items = await query.Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                                .Take(paginationParams.PageSize)
                                .ToListAsync();
 
        var pagedResponse = new PagedResponse<Trainee>(items, paginationParams.PageNumber, paginationParams.PageSize, totalRecords);
 
        return pagedResponse;
    }

}