using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.LearningTaskDTOs;

public enum LearningTaskStatus
{
    Draft,
    Published,
    Closed
}

public class CreateLearningTaskRequestDTO
{
    [Required(ErrorMessage = "Title is required")]
    [MaxLength(50)]
    [MinLength(1)]    
    public required string Title { get; set; }

    [Required(ErrorMessage = "Description is required")]
    [MaxLength(100)]
    [MinLength(1)]
    public required string Description { get; set; }

    [Required(ErrorMessage = "ExpectedTechStack is required")]
    [MaxLength(50)]
    [MinLength(1)]
    public required string ExpectedTechStack { get; set; }

    [Required(ErrorMessage = "DueDate is required")]
    public required DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Status can only be Draft, Published or Closed.")]
    public required LearningTaskStatus Status { get; set; }
}