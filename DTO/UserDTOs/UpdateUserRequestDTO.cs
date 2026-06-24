using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.UserDTOs;

[Index(nameof(Username), IsUnique = true)]
public class UpdateUserRequestDTO
{
    [Required]
    public String? Username { get; set; } // unique

    [Required]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    public String? Email { get; set; }
    
    [Required]
    public String?  Password { get; set; } // Something

    [Required]
    [EnumDataType(typeof(Role), ErrorMessage = "Role can only be Admin, Mentor or Trainee")]
    public Role? Role { get; set; }

}
