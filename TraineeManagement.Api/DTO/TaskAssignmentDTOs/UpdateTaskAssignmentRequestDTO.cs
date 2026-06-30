using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.TaskAssignmentDTOs;

public class UpdateTaskAssignmentRequestDTO
{
    public TaskAssignmentStatus? Status { get; set; }
}