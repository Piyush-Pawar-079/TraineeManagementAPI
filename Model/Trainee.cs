
using traineeManagementAPI.DTO.TraineeDTOs;

namespace traineeManagementAPI.Model;
    public class Trainee
{
    public int Id { get; set; } // auto-generated
    public required String FirstName { get; set; } 
    public required String LastName { get; set; }
    public required String Email { get; set; }
    public required String  TechStack { get; set; }
    public required Status Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
    
}
