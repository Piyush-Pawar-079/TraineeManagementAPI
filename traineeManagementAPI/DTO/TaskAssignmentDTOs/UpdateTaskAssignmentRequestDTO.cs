using CommonLibrary.Models;

namespace traineeManagementAPI.DTO.TaskAssignmentDTOs;

public class UpdateTaskAssignmentRequestDTO
{
    public TaskAssignmentStatus? Status { get; set; } 
}