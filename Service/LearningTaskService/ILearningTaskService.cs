using traineeManagementAPI.DTO.LearningTaskDTOs;

namespace traineeManagementAPI.Service.LearningTaskService;

public interface ILearningTaskService
{
    Task<List<LearningTaskResponseDTO>> GetAllAsync();

    Task<LearningTaskResponseDTO?> GetByIdAsync(int id);

    Task<LearningTaskResponseDTO> CreateAsync(CreateLearningTaskRequestDTO createLearningTaskDto);

    Task<LearningTaskResponseDTO?> UpdateAsync(int id, UpdateLearningTaskRequestDTO updateLearningTaskDto);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 
}