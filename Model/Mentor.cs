namespace traineeManagementAPI.Model;

public enum MentorStatus
{
    Active,
    Inactive
}

public class Mentor
{
    public int Id { get; set; } // auto-generated
    public required string FirstName { get; set; } 
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Expertise { get; set; }
    public required MentorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
    public List<TaskAssignment> TaskAssignments { get; set; } = [];
    public List<Review> Reviews { get; set; } = [];
}