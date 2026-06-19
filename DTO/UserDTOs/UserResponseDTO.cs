using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.UserDTOs;

public class UserResponseDTO
{
    public int Id { get; set; } // auto-generated
    public required String Username { get; set; } // unique
    public required String Email { get; set; }

    // public required String  PasswordHash { get; set; }  we never send user their password. 
    public required Role Role { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
}