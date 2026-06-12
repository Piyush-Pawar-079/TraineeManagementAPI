using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.TaskAssignmentRepository;

public interface ITaskAssignmentRepository
{
    
    Task<List<TaskAssignment>> GetAllTaskAssignmentAsync();

    Task<TaskAssignment?> GetTaskAssignmentByIdAsync(int id);

    Task<TaskAssignment> CreateTaskAssignmentAsync(TaskAssignment taskAssignment);

    Task<TaskAssignment?> UpdateTaskAssignmentAsync(int id, TaskAssignment taskAssignment);

}