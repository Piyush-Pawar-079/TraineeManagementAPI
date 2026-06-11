using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.AuthDTOs;
using traineeManagementAPI.DTO.UserDTOs;
using traineeManagementAPI.Service.AuthService;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("/api/auth")]
public class AuthController(IAuthService authService, ILogger<AuthController> logger) : ControllerBase
{
    
    private readonly IAuthService _authService = authService;
    private readonly ILogger<AuthController> _logger = logger;

    [HttpPost("/register")]
    public async Task<ActionResult<UserResponseDTO>> RegisterUser(CreateUserRequestDTO createUserDTO)
    {
        var response = await _authService.Register(createUserDTO);

        if (response == null)
        {
            _logger.LogError("User registration failed");
            return BadRequest("User with a similar username already exists");
        }

        _logger.LogInformation("User registered successfully");
        return Ok(response);
    }

    [HttpPost("/login")]
    public async Task<ActionResult<UserResponseDTO>> LoginUser(LoginDTO loginDTO)
    {
        var response = await _authService.Login(loginDTO);

        if (response == null)
        {
            _logger.LogError("User login failed");
            return BadRequest();
        }
        _logger.LogInformation("User login successfull");
        return Ok(response);
    }

}