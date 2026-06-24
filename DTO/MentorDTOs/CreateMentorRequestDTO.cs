using System.ComponentModel.DataAnnotations;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.MentorDTOs;

public class CreateMentorRequestDTO
{
    [Required(ErrorMessage = "First name is required")]
    public required string FirstName { set; get; }


    [Required(ErrorMessage = "Last name is required")]
    public required string LastName { set; get; }

    [Required]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public required string Email { set; get; }

    [Required(ErrorMessage = "Expertise is required.")]
    public required string Expertise { set; get; }

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Status can only be Active or Inactive")]
    public required MentorStatus Status { set; get; }
}
