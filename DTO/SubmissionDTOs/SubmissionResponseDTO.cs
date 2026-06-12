using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

public class SubmissionResponseDTO
{
    public int Id { get; set; }
    public int TaskAssignmentId { get; set; }
    public TaskAssignment? TaskAssignment { get; set; }
    public string SubmissionUrl { get; set; } = String.Empty;
    public string Notes { get; set; } = String.Empty;
    public DateTime SubmittedDate { get; set; }
    public string Status { get; set; } = String.Empty;
}