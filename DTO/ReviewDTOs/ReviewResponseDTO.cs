using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.ReviewDTOs;

public class ReviewResponseDTO
{
    public int Id { get; set; }
    public int SubmissionId { get; set; } 
    public SubmissionResponseDTO? Submission { get; set; }
    public int MentorId { get; set; }
    public MentorResponseDTO? Mentor { get; set; }
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required ReviewStatus ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}