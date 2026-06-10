using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.MentorDTOs;

public enum MentorStatus
{
    Active,
    Inactive
}
public class CreateMentorRequestDTO
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { set; get; }


    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { set; get; }

    [Required]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public required string Email { set; get; }

    [Required]
    public required string Expertise { set; get; }

    [Required]
    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Status can only be Active or Inactive")]
    public required string Status { set; get; }
}
