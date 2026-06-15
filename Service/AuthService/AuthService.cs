using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using traineeManagementAPI.DTO.AuthDTOs;
using traineeManagementAPI.DTO.UserDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.UserRepository;

namespace traineeManagementAPI.Service.AuthService;

public class AuthService(IUserRepository repository, IConfiguration configuration) : IAuthService
{

    private static int _nextId = 0;
    private readonly IUserRepository _repository = repository;

    private static UserResponseDTO MapToUserResponseDTO(User user)
    {
        return new UserResponseDTO
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
            CreatedDate = user.CreatedDate,
            UpdatedDate = user.UpdatedDate
        };
    }

    public async Task<UserResponseDTO?> Register(CreateUserRequestDTO createUserRequestDTO)
    {
        var usersList = await _repository.GetAllAsync();

        var foundUser = usersList.FirstOrDefault(u => u.Username == createUserRequestDTO.Username);

        if (foundUser != null)
        {
            return null;
        }

        string HashedPassword = new PasswordHasher<CreateUserRequestDTO>()
        .HashPassword(createUserRequestDTO, createUserRequestDTO.Password);

        var newUser = new User
        {
            Id = _nextId++,
            Username = createUserRequestDTO.Username,
            PasswordHash = HashedPassword,
            Email = createUserRequestDTO.Email,
            Role = createUserRequestDTO.Role,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _nextId++;

        var createdUser = await _repository.CreateAsync(newUser);

        return MapToUserResponseDTO(createdUser);
    }

    public async Task<LoginResponseDTO?> Login(LoginDTO loginDTO)
    {
        var usersList = await _repository.GetAllAsync();

        var found = usersList.FirstOrDefault(u => u.Username == loginDTO.Username);

        if (found == null)
        {
            return null;
        }

        CreateUserRequestDTO userRequestDTO = new CreateUserRequestDTO
        {
            Username = found.Username,
            Password = found.PasswordHash,
            Email = found.Email,
            Role = found.Role
        };

        if (new PasswordHasher<CreateUserRequestDTO>().VerifyHashedPassword(userRequestDTO, userRequestDTO.Password, loginDTO.Password)
            == PasswordVerificationResult.Failed
        )
        {
            return null;
        }

        String token = GenerateToken(userRequestDTO);

        return new LoginResponseDTO
        {
            Token = token,
            ExpiresIn = DateTime.UtcNow.AddMinutes(60),
            User = MapToUserResponseDTO(found)
        };
    }

    private String GenerateToken(CreateUserRequestDTO userRequestDTO)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, userRequestDTO.Username),
            new Claim(ClaimTypes.Email, userRequestDTO.Email),
            new Claim(ClaimTypes.Role, userRequestDTO.Role.ToString())
        };

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration.GetValue<string>("Token:Key")!)
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