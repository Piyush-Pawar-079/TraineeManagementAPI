using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.TaskAssignmentDTOs;

public class TaskAssignmentResponseDTO
{
    public int Id { get; set; } // auto-generated
    public required int TraineeId { get; set; }
    public Trainee? Trainee { get; set; }
    public required int MentorId { get; set; }
    public Mentor? Mentor { get; set; }
    public required int LearningTaskId { get; set; }
    public LearningTask? LearningTask { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}