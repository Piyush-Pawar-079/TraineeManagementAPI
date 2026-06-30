namespace TraineeManagement.Api.DTO.TaskAssignmentDTOs;

public class TaskAssignmentBasicDTO
{

    public int Id { get; set; }
    public required string TraineeName { get; set; }
    public required string MentorName { get; set; }
    public required string LearningTaskTitle { get; set; }
    public DateTime DueDate { get; set; }
    public required string Status { get; set; }

}