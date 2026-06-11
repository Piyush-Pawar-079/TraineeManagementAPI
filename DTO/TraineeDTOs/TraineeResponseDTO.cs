namespace traineeManagementAPI.DTO.TraineeDTOs;

public class TraineeResponseDTO
{

    public int Id { set; get; }
    public string FirstName { set; get; } = string.Empty;

    public string LastName { set; get; } = string.Empty;

    public string Email { set; get; } = string.Empty;

    public string TechStack { set; get; } = string.Empty;

    public Status Status { set; get; } 

    public DateTime CreateDate { set; get; }

    public DateTime UpdateDate { set; get; }

}
