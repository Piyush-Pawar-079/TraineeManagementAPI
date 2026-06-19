using Microsoft.EntityFrameworkCore;

namespace traineeManagementAPI.Model;

public enum Role
{
    Admin,
    Mentor, 
    Trainee
}


[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; } // auto-generated
    public required string Username { get; set; } // unique
    public required string Email { get; set; }
    public required string  PasswordHash { get; set; }
    public required Role Role { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated

}