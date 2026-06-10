namespace traineeManagementAPI.Model;

public class TaskAssignment
{
    public int Id { get; set; } // auto-generated
    public int TraineeId { get; set; }
    public int MentorId { get; set; }
    public int LearningTaskId { get; set; }
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public String Status { get; set; } = String.Empty;
    public String? Remarks { get; set; }
}