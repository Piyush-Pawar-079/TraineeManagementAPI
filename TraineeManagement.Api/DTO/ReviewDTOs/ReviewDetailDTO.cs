using TraineeManagement.Api.DTO.MentorDTOs;
using TraineeManagement.Api.DTO.SubmissionDTOs;
using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.ReviewDTOs;

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