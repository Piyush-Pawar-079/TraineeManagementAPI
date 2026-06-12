
namespace traineeManagementAPI.Model;
    public class Trainee
{
    public int Id { get; set; } // auto-generated
    public required string FirstName { get; set; } 
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string  TechStack { get; set; }
    public required string Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
    public ICollection<TaskAssignment> TaskAssignments { get; set; } = [];
    
}
