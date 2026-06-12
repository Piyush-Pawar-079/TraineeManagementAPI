using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.ReviewDTOs;

public class ReviewResponseDTO
{
    public int Id { get; set; }
    public int SubmissionId { get; set; } 
    public Submission? Submission { get; set; }
    public int MentorId { get; set; }
    public Mentor? Mentor { get; set; }
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required string ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}