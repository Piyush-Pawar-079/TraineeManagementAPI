using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.TraineeDTOs;

public class TraineeDetailDTO
{

    public int Id { set; get; }
    public string FirstName { set; get; } = string.Empty;
    public string LastName { set; get; } = string.Empty;
    public string Email { set; get; } = string.Empty;
    public string TechStack { set; get; } = string.Empty;
    public Status Status { set; get; }
    public DateTime CreatedDate { set; get; }
    public DateTime UpdatedDate { set; get; }
}
