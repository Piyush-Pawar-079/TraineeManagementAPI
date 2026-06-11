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
        var response = await _authService.Register(createUserDTO);

        if (response == null)
        {
            return BadRequest("User with a similar username already exists");
        }

        return Ok(response);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<UserResponseDTO>> LoginUser(LoginDTO loginDTO)
    {
        var response = await _authService.Login(loginDTO);

        if (response == null)
        {
            return BadRequest();
        }
        return Ok(response);
    }

}