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
    [Required(ErrorMessage = "Username is required.")]
    public required String Username { get; set; } // unique

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public required String Email { get; set; }
    
    [Required(ErrorMessage = "Password is required.")]
    public required String  Password { get; set; }

    [Required(ErrorMessage = "Role is required.")]
    [EnumDataType(typeof(Role), ErrorMessage = "Role can only be Admin, Mentor or Trainee")]
    public required String Role { get; set; }

}
