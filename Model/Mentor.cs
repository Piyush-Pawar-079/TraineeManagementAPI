using traineeManagementAPI.DTO.MentorDTOs;

namespace traineeManagementAPI.Model;

public class Mentor
{
    public int Id { get; set; } // auto-generated
    public required String FirstName { get; set; } 
    public required String LastName { get; set; }
    public required String Email { get; set; }
    public required String  Expertise { get; set; }
    public required MentorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
}