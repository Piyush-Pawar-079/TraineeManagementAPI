using traineeManagementAPI.DTO.TaskAssignmentDTOs;

namespace traineeManagementAPI.Service.TaskAssignmentService;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentResponseDTO>> GetAllAsync();

    Task<TaskAssignmentResponseDTO?> GetByIdAsync(int id);

    Task<TaskAssignmentResponseDTO> CreateAsync(CreateTaskAssignmentRequestDTO createTaskAssignmentDto);

    Task<TaskAssignmentResponseDTO?> UpdateAsync(int id, UpdateTaskAssignmentRequestDTO updateTaskAssignmentDto);
}