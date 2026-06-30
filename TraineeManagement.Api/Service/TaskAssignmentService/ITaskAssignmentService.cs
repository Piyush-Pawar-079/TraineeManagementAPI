using TraineeManagement.Api.DTO.TaskAssignmentDTOs;

namespace TraineeManagement.Api.Service.TaskAssignmentService;

public interface ITaskAssignmentService
{
    Task<List<TaskAssignmentDetailDTO>> GetAllAsync();

    Task<TaskAssignmentDetailDTO?> GetByIdAsync(int id);

    Task<TaskAssignmentDetailDTO> CreateAsync(CreateTaskAssignmentRequestDTO createTaskAssignmentDto);

    Task<TaskAssignmentDetailDTO?> UpdateAsync(int id, UpdateTaskAssignmentRequestDTO updateTaskAssignmentDto);
}