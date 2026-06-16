using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace traineeManagementAPI.Model;

public enum TaskAssigmentStatus
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
    [JsonIgnore]
    public Trainee? Trainee { get; set; }
    public required int MentorId { get; set; }
    [JsonIgnore]
    public Mentor? Mentor { get; set; }
    public required int LearningTaskId { get; set; } 
    [JsonIgnore]
    public LearningTask? LearningTask { get; set; }
    public List<Submission> Submission { get; set; } = [];
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public TaskAssigmentStatus Status { get; set; }
    public string? Remarks { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)  
        {  
            if(DueDate <= AssignedDate)  
            {  
                yield return new ValidationResult("Assigned date must be greater than the Due date.", new[] { "Assigned Date" });  
            }  
        }
}