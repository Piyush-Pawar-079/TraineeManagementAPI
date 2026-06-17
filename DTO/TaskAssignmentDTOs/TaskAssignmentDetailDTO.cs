using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.TraineeDTOs;

namespace traineeManagementAPI.DTO.TaskAssignmentDTOs;

public class TaskAssignmentDetailDTO
{
    public int Id { get; set; }

    public TraineeBasicDTO Trainee { get; set; } = null!;
    public MentorBasicDTO Mentor { get; set; } = null!;
    public LearningTaskBasicDTO LearningTask { get; set; } = null!;

    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public required string Status { get; set; }
    public string? Remarks { get; set; }
}