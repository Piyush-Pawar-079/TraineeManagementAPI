using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.LearningTaskDTOs;

public class UpdateLearningTaskRequestDTO
{
    [MaxLength(50)]
    [MinLength(1)]
    public string? Title { get; set; }

    [MaxLength(100)]
    [MinLength(1)]
    public string? Description { get; set; }

    [MaxLength(50)]
    [MinLength(1)]
    public string? ExpectedTechStack { get; set; }

    public DateTime? DueDate;

    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Status can only be Draft, Published or Closed.")]
    public LearningTaskStatus? Status { get; set; }
}