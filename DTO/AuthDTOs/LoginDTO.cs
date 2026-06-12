using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.AuthDTOs;

public class LoginDTO
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { set; get; } = String.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { set; get; } = String.Empty;
}