using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.TraineeDTOs;

public enum Status
{
    Active,
    Inactive,
    Completed
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
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public string Email { set; get; } = string.Empty;

    [Required]
    public string TechStack { set; get; } = string.Empty;

    [Required]
    [EnumDataType(typeof(Status), ErrorMessage = "Status can only be Active, Inactive or Completed")]
    public Status Status { set; get; }

}
