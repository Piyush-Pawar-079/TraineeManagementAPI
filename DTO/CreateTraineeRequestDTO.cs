using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO;

public enum Status
{
    Active,
    Inactive
}
public class CreateTraineeRequestDTO
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50)]
    public string FirstName { set; get; } = string.Empty;


    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50)]
    public string LastName { set; get; } = string.Empty;

    [Required]
    [MinLength(3)]
    [MaxLength(50)]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    // Also add validation for email format.
    public string Email { set; get; } = string.Empty;

    [Required]
    public string TechStack { set; get; } = string.Empty;

    [Required]
    [MaxLength(50)]
    [EnumDataType(typeof(Status), ErrorMessage = "Status can only be active or inactive")]
    public string Status { set; get; } = string.Empty;

}
