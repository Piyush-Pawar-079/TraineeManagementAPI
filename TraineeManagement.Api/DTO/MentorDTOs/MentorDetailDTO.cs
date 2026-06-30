using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.MentorDTOs;

public class MentorDetailDTO
{
    public int Id { get; set; } // auto-generated
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string Expertise { get; set; }
    public required MentorStatus Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated

}