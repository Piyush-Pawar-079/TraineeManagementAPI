using System.ComponentModel.DataAnnotations;

namespace traineeManagementAPI.DTO.LearningTaskDTOs;

public class UpdateLearningTaskRequestDTO
{
    public String? Title;

    public String? Description;

    public String? ExpectedTechStack;

    public DateTime? DueDate;

    [EnumDataType(typeof(LearningTaskStatus), ErrorMessage = "Status can only be Draft, Published or Closed.")]
    public String? Status;
}