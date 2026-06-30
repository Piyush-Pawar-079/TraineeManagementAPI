using TraineeManagement.Api.DTO.AuthDTOs;
using TraineeManagement.Api.DTO.UserDTOs;

namespace TraineeManagement.Api.Service.AuthService;

public interface IAuthService
{
    public Task<UserResponseDTO?> Register(CreateUserRequestDTO createUserDTO);

    public Task<LoginResponseDTO?> Login(LoginDTO loginDTO);

}