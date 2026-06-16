using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

public class SubmissionResponseDTO
{
    public int Id { get; set; }
    public int TaskAssignmentId { get; set; }
    public TaskAssignmentResponseDTO? TaskAssignment { get; set; } 
    public List<ReviewResponseDTO> Reviews { get; set; } = [];
    public string SubmissionUrl { get; set; } = String.Empty;
    public string Notes { get; set; } = String.Empty;
    public DateTime SubmittedDate { get; set; }
    public SubmissionStatus Status { get; set; }
}