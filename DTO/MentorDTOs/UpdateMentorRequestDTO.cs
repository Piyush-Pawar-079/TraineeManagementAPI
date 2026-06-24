using System.ComponentModel.DataAnnotations;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.MentorDTOs;

public class UpdateMentorRequestDTO
{
    public string? FirstName { set; get; }
    public string? LastName { set; get; }

    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public string? Email { set; get; }

    public string? Expertise { set; get; }

    [EnumDataType(typeof(MentorStatus), ErrorMessage = "Status can only be Active or Inactive")]
    public MentorStatus? Status { set; get; }
}
