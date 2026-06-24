using traineeManagementAPI.DTO.UserDTOs;

namespace traineeManagementAPI.DTO.AuthDTOs;

public class LoginResponseDTO
{
    public required String Token { set; get; } = String.Empty;

    public DateTime ExpiresIn { set; get; }

    public required UserResponseDTO User { set; get; }
}