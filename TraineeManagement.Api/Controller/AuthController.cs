using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.AuthDTOs;
using TraineeManagement.Api.DTO.UserDTOs;
using TraineeManagement.Api.Service.AuthService;

namespace TraineeManagement.Api.Controller;

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