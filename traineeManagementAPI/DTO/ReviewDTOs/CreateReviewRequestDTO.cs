using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;

namespace traineeManagementAPI.DTO.ReviewDTOs;

public class CreateReviewRequestDTO
{
    [Required(ErrorMessage = "Submission Id is required")]
    public required int SubmissionId { get; set; } 
    
    [Required(ErrorMessage = "Mentor Id is required")]
    public required int MentorId { get; set; }
    
    [Required(ErrorMessage = "Feedback is required")]
    [MinLength(1)]
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    
    [Required(ErrorMessage = "Status is required")]
    [EnumDataType(typeof(ReviewStatus), ErrorMessage = "Review status can only be Accepted, ChangesRequired or Rejected")]
    public required ReviewStatus ReviewStatus { get; set; }

    [Required(ErrorMessage = "ReviewDate is required")]
    public DateTime ReviewedDate { get; set; } 
}