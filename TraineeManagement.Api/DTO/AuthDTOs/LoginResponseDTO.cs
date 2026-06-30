using TraineeManagement.Api.DTO.UserDTOs;

namespace TraineeManagement.Api.DTO.AuthDTOs;

public class LoginResponseDTO
{
    public required String Token { set; get; } = String.Empty;

    public DateTime ExpiresIn { set; get; }

    public required UserResponseDTO User { set; get; }
}