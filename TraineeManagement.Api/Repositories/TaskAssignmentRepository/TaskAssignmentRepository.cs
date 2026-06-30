using Microsoft.EntityFrameworkCore;
using CommonLibrary.Models;
using CommonLibrary.Data;

namespace TraineeManagement.Api.Repositories.TaskAssignmentRepository;

public class TaskAssignmentRepository(ApplicationDBContext context) : ITaskAssignmentRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<TaskAssignment>> GetAllTaskAssignmentAsync()
    {
        return await _context.TaskAssignments.Include(ta => ta.Trainee).Include(ta => ta.Mentor).Include(ta => ta.LearningTask)
            .Include(ta => ta.Submission).ThenInclude(s => s.Reviews).ToListAsync();
    }

    public async Task<TaskAssignment?> GetTaskAssignmentByIdAsync(int id)
    {
        return await _context.TaskAssignments.Include(ta => ta.Trainee).Include(ta => ta.Mentor).Include(ta => ta.LearningTask)
            .Include(ta => ta.Submission).FirstOrDefaultAsync(ta => ta.Id == id);
    }

    public async Task<TaskAssignment> CreateTaskAssignmentAsync(TaskAssignment taskAssignment)
    {
        _context.TaskAssignments.Add(taskAssignment);
        await _context.SaveChangesAsync();
        return taskAssignment;
    }

    public async Task<TaskAssignment?> UpdateTaskAssignmentAsync(int id, TaskAssignment taskAssignment)
    {
        _context.TaskAssignments.Update(taskAssignment);
        await _context.SaveChangesAsync();
        return await GetTaskAssignmentByIdAsync(id);
    }

}