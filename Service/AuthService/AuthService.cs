using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using traineeManagementAPI.DTO.AuthDTOs;
using traineeManagementAPI.DTO.UserDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.UserRepository;

namespace traineeManagementAPI.Service.AuthService;

public class AuthService(IUserRepository repository, IConfiguration configuration, IMapper mapper, ILogger<AuthService> logger) : IAuthService
{
    private readonly IUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly ILogger<AuthService> _logger = logger;

    public async Task<UserResponseDTO?> Register(CreateUserRequestDTO createUserRequestDTO)
    {
        var usersList = await _repository.GetAllAsync();

        var foundUser = usersList.FirstOrDefault(u => u.Username == createUserRequestDTO.Username);

        if (foundUser != null)
        {
            _logger.LogError("User registration failed");
            throw new BadRequestException("User with the same username already exists");
        }

        string HashedPassword = new PasswordHasher<CreateUserRequestDTO>()
        .HashPassword(createUserRequestDTO, createUserRequestDTO.Password);

        // var newUser = new User
        // {
        //     Id = _nextId++,
        //     Username = createUserRequestDTO.Username,
        //     PasswordHash = HashedPassword,
        //     Email = createUserRequestDTO.Email,
        //     Role = createUserRequestDTO.Role,
        //     CreatedDate = DateTime.UtcNow,
        //     UpdatedDate = DateTime.UtcNow
        // };

        var newUser = _mapper.Map<User>(createUserRequestDTO);

        var createdUser = await _repository.CreateAsync(newUser);
        
        _logger.LogInformation("User registered successfully");
        return _mapper.Map<UserResponseDTO>(createdUser);
    }

    public async Task<LoginResponseDTO?> Login(LoginDTO loginDTO)
    {
        var usersList = await _repository.GetAllAsync();

        var found = usersList.FirstOrDefault(u => u.Username == loginDTO.Username);

        if (found == null)
        {
            _logger.LogError("User login failed");
            throw new NotFoundException($"User with the username {loginDTO.Username} not found");
        }

        CreateUserRequestDTO userRequestDTO = new()
        {
            Username = found.Username,
            Password = found.PasswordHash,
            Email = found.Email,
            Role = found.Role
        };

        // var userRequestDTO = _mapper.Map<CreateUserRequestDTO>(found);

        if (new PasswordHasher<CreateUserRequestDTO>().VerifyHashedPassword(userRequestDTO, userRequestDTO.Password, loginDTO.Password)
            == PasswordVerificationResult.Failed
        )
        {
            _logger.LogInformation("User login failed because of wrong password");
            throw new BadRequestException("Password Incorrect");
        }

        string token = GenerateToken(userRequestDTO);

        _logger.LogInformation("User login successfull");
        return new LoginResponseDTO
        {
            Token = token,
            ExpiresIn = DateTime.UtcNow.AddMinutes(60),
            User = _mapper.Map<UserResponseDTO>(found)
        };
    }

    private string GenerateToken(CreateUserRequestDTO userRequestDTO)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userRequestDTO.Username),
            new Claim(ClaimTypes.Email, userRequestDTO.Email),
            new Claim(ClaimTypes.Role, userRequestDTO.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key")!)
        );

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

        var tokenDescriptor = new JwtSecurityToken(
            issuer: configuration.GetValue<string>("Token:Issuer"),
            audience: configuration.GetValue<string>("Token:Audience"),
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

    }

}