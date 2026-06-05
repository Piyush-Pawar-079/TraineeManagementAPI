using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;

namespace traineeManagementAPI.DTO;

public class UpdateTraineeRequestDTO
{
    [MaxLength(50)]
    public string? FirstName { set; get; }


    [MaxLength(50)]
    public string? LastName { set; get; }

    [MaxLength(50)]
    [EmailAddress(ErrorMessage = "Valid Email is required")]
    // Also add validation for email format.
    public string? Email { set; get; }


    public string? TechStack { set; get; }

    [MaxLength(50)]
    // Add validation for valid status. 
    public string? Status { set; get; } 
    
}
