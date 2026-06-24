using traineeManagementAPI.DTO.AuthDTOs;
using traineeManagementAPI.DTO.UserDTOs;

namespace traineeManagementAPI.Service.AuthService;

public interface IAuthService
{
    public Task<UserResponseDTO?> Register(CreateUserRequestDTO createUserDTO);

    public Task<LoginResponseDTO?> Login(LoginDTO loginDTO);
    
}