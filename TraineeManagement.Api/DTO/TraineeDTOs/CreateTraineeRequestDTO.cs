using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.TraineeDTOs;

public class CreateTraineeRequestDTO
{
    [Required(ErrorMessage = "First name is required")]
    [MaxLength(50)]
    [MinLength(1)]
    public string FirstName { set; get; } = string.Empty;


    [Required(ErrorMessage = "Last name is required")]
    [MaxLength(50)]
    [MinLength(1)]
    public string LastName { set; get; } = string.Empty;

    [Required]
    [MinLength(1)]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public string Email { set; get; } = string.Empty;

    [Required(ErrorMessage = "Tech stack is required.")]
    [MinLength(1)]
    public string TechStack { set; get; } = string.Empty;

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(Status), ErrorMessage = "Status can only be Active, Inactive or Completed")]
    public required Status Status { get; set; }

}
