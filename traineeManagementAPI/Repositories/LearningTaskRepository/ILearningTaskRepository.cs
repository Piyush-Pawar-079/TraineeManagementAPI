using CommonLibrary.Models;

namespace traineeManagementAPI.Repositories.LearningTaskRepository;

public interface ILearningTaskRepository
{
    Task<List<LearningTask>> GetAllAsync();

    Task<LearningTask?> GetByIdAsync(int id);

    Task<LearningTask> CreateAsync(LearningTask LearningTask);

    Task<LearningTask?> UpdateAsync(int id, LearningTask LearningTask);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 
}