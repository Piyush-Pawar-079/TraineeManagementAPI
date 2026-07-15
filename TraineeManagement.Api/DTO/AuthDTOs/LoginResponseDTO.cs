using TraineeManagement.Api.DTO.UserDTOs;

namespace TraineeManagement.Api.DTO.AuthDTOs;

public class LoginResponseDTO
{
    public required string Token { set; get; } = "";

    public DateTime ExpiresIn { set; get; }

    public required UserResponseDTO User { set; get; }
}