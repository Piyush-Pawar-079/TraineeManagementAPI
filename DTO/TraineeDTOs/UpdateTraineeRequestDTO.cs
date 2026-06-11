using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.TraineeDTOs;


public class UpdateTraineeRequestDTO
{
    [MaxLength(50)]
    public string? FirstName { set; get; }


    [MaxLength(50)]
    public string? LastName { set; get; }

    [EmailAddress(ErrorMessage = "Valid Email is required")]
    // Also add validation for email format.
    public string? Email { set; get; }


    public string? TechStack { set; get; }

    [EnumDataType(typeof(Status), ErrorMessage = "Status can only be Active, Inactive or Completed")]
    public Status? Status { set; get; } 
    
}
