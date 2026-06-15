using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.Model;

public enum TaskAssignemntStatus
{
    Assigned,
    InProgress,
    Submitted,
    Reviewed,
    Completed 
}

public class TaskAssignment : IValidatableObject
{
    public int Id { get; set; } // auto-generated
    public required int TraineeId { get; set; }
    public Trainee? Trainee { get; set; }
    public required int MentorId { get; set; }
    public Mentor? Mentor { get; set; }
    public required int LearningTaskId { get; set; }
    public LearningTask? LearningTask { get; set; }
    public List<Submission> Submission { get; set; } = [];
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Remarks { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  
        {  
            if(DueDate <= AssignedDate)  
            {  
                yield return new ValidationResult("Assigned date must be greater than the Due date.", new[] { "Assigned Date" });  
            }  
        }
}