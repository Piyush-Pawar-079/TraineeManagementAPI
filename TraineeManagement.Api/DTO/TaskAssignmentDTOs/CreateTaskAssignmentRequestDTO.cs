using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.TaskAssignmentDTOs;

public class CreateTaskAssignmentRequestDTO
{
    [Required(ErrorMessage = "TraineedId is required.")]
    public int TraineeId { get; set; }

    [Required(ErrorMessage = "MentorId is required.")]
    public int MentorId { get; set; }

    [Required(ErrorMessage = "LearningTaskId is required.")]
    public int LearningTaskId { get; set; }

    [Required(ErrorMessage = "AssignedDate is required.")]
    public DateTime AssignedDate { get; set; }

    [Required(ErrorMessage = "DueDate is required.")]
    public DateTime DueDate { get; set; }

    [Required(ErrorMessage = "Status is required.")]
    [EnumDataType(typeof(TaskAssignmentStatus), ErrorMessage = "Status can only be Assigned, InProgress, Submitted, Reviewed and Completed")]
    public TaskAssignmentStatus Status { get; set; }
    public string? Remarks { get; set; }
}