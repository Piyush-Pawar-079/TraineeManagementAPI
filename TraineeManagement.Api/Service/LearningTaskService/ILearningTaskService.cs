using TraineeManagement.Api.DTO.LearningTaskDTOs;

namespace TraineeManagement.Api.Service.LearningTaskService;

public interface ILearningTaskService
{
    Task<List<LearningTaskDetailDTO>> GetAllAsync();

    Task<LearningTaskDetailDTO?> GetByIdAsync(int id);

    Task<LearningTaskDetailDTO> CreateAsync(CreateLearningTaskRequestDTO createLearningTaskDto);

    Task<LearningTaskDetailDTO?> UpdateAsync(int id, UpdateLearningTaskRequestDTO updateLearningTaskDto);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync();
}