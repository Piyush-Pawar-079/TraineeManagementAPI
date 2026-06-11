using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.AuthDTOs;

public class LoginDTO
{
    [Required]
    public string Username { set; get; } = String.Empty;

    [Required]
    public string Password { set; get; } = String.Empty;
}