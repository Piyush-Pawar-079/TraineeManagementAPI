namespace traineeManagementAPI.DTO.LearningTaskDTOs;

public class LearningTaskResponseDTO
{
     public int Id { get; set; } // auto-generated
    public required String Title { get; set; } 
    public required String Description { get; set; }
    public required String  ExpectedTechStack { get; set; }
    public required DateTime DueDate { get; set; }
    public required String Status { get; set; }
    public DateTime CreatedDate { get; set; } // auto-generated
    public DateTime UpdatedDate { get; set; } // auto-generated
}