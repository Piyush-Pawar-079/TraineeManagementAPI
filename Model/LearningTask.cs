
namespace traineeManagementAPI.Model;

public class LearningTask
{
    public int Id { get; set; } // auto-generated
    public required string Title { get; set; } 
    public required string Description { get; set; }
    public required string  ExpectedTechStack { get; set; }
    public required DateTime DueDate { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = [];

}