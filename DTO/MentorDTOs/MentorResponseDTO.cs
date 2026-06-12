using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.MentorDTOs;

public class MentorResponseDTO
{
    public int Id { get; set; } // auto-generated
    public required string FirstName { get; set; } 
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string  Expertise { get; set; }
    public required string Status { get; set; }
    public required List<TaskAssignment> TaskAssignmentId { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
}