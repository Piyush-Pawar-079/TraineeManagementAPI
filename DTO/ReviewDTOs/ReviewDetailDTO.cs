using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.ReviewDTOs;

public class ReviewDetailDTO
{
    public int Id { get; set; }
    public SubmissionBasicDTO Submission { get; set; } = null!;
    public MentorBasicDTO Mentor { get; set; } = null!;
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required ReviewStatus ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}