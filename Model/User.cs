using Microsoft.EntityFrameworkCore;

namespace traineeManagementAPI.Model;

[Index(nameof(Username), IsUnique = true)]
public class User
{
    public int Id { get; set; } // auto-generated
    public required String Username { get; set; } // unique
    public required String Email { get; set; }
    public required String  PasswordHash { get; set; }
    public required String Role { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated

}