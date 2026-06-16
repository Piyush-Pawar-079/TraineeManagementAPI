using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.TraineeDTOs;


public class UpdateTraineeRequestDTO
{
    [MaxLength(50)]
    [MinLength(1)]
    public string? FirstName { set; get; }

    [MaxLength(50)]
    [MinLength(1)]
    public string? LastName { set; get; }

    [EmailAddress(ErrorMessage = "Valid Email is required")]
    [MinLength(1)]
    public string? Email { set; get; }


    [MinLength(1)]
    public string? TechStack { set; get; }

    [EnumDataType(typeof(Status), ErrorMessage = "Status can only be Active, Inactive or Completed")]
    public Status? Status { set; get; } 
    
}
