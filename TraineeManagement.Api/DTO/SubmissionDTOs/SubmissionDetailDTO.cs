using TraineeManagement.Api.DTO.TaskAssignmentDTOs;
using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.SubmissionDTOs;

public class SubmissionDetailDTO
{
    public int Id { get; set; }
    public int TaskAssignmentId { get; set; }
    public TaskAssignmentBasicDTO TaskAssignment { get; set; } = null!;
    public string SubmissionUrl { get; set; } = String.Empty;
    public string Notes { get; set; } = String.Empty;
    public DateTime SubmittedDate { get; set; }
    public SubmissionStatus Status { get; set; }
}