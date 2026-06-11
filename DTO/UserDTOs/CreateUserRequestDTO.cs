using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace traineeManagementAPI.DTO.UserDTOs;

public enum Role
{
    Admin,
    Mentor, 
    Trainee
}

[Index(nameof(Username), IsUnique = true)]
public class CreateUserRequestDTO
{
    [Required]
    public required String Username { get; set; } // unique

    [Required]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public required String Email { get; set; }
    
    [Required]
    public required String  Password { get; set; }

    [Required]
    [EnumDataType(typeof(Role), ErrorMessage = "Role can only be Admin, Mentor or Trainee")]
    public required Role Role { get; set; }

}
