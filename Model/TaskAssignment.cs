namespace traineeManagementAPI.Model;

public class TaskAssignment
{
    public int Id { get; set; } // auto-generated
    public int TraineeId { get; set; }
    public int MentorId { get; set; }
    public int LearningTaskId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }
}