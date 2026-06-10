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
    [Required]
    public required String Title;

    [Required]
    public required String Description;

    [Required]
    public required String ExpectedTechStack;

    [Required]
    public required DateTime DueDate;

    [Required]
    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Status can only be Draft, Published or Closed.")]
    public required String Status;
}