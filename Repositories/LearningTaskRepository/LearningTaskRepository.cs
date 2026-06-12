using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.LearningTaskRepository;

public class LearningTaskRepository(ApplicationDBContext context) : ILearningTaskRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<LearningTask>> GetAllAsync()
    {
        return await _context.LearningTasks.ToListAsync();
    }

    public async Task<LearningTask?> GetByIdAsync(int id)
    {
        return await _context.LearningTasks.FindAsync(id);
    }

    public async Task<LearningTask> CreateAsync(LearningTask LearningTask)
    {
        await _context.LearningTasks.AddAsync(LearningTask);
        await _context.SaveChangesAsync();

        return LearningTask;
    }

    public async Task<LearningTask?> UpdateAsync(int id, LearningTask LearningTask)
    {
        _context.LearningTasks.Update(LearningTask);
        await _context.SaveChangesAsync();

        return await _context.LearningTasks.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var LearningTask = await _context.LearningTasks.FindAsync(id);
        if (LearningTask == null)
        {
            return false;
        }
        _context.LearningTasks.Remove(LearningTask);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}