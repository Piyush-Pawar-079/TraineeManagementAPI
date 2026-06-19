using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.AuthDTOs;
using traineeManagementAPI.DTO.UserDTOs;
using traineeManagementAPI.Service.AuthService;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService authService) : ControllerBase
{
    
    private readonly IAuthService _authService = authService;

    [HttpPost("/register")]
    public async Task<ActionResult<UserResponseDTO>> RegisterUser(CreateUserRequestDTO createUserDTO)
    {
        return Ok(await _authService.Register(createUserDTO));
    }

    [HttpPost("/login")]
    public async Task<ActionResult<UserResponseDTO>> LoginUser(LoginDTO loginDTO)
    {
        return Ok(await _authService.Login(loginDTO));
    }

}