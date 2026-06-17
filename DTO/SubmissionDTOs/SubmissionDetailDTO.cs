using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

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