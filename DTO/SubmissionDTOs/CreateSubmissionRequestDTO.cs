using System.ComponentModel.DataAnnotations;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

public class CreateSubmissionRequestDTO
{
    [Required]
    public int TaskAssignmentId { get; set; }
    
    [Required]
    [MaxLength(50)]
    [MinLength(1)]
    public required string SubmissionUrl { get; set; }
    
    [Required]
    [MaxLength(100)]
    [MinLength(1)]
    public required string Notes { get; set; }

    [Required]
    public required DateTime SubmittedDate { get; set; }
    
    [Required]
    [EnumDataType(typeof(SubmissionStatus), ErrorMessage = "Status can only be Submitted or Resubmited")]
    public required SubmissionStatus Status { get; set; }
}