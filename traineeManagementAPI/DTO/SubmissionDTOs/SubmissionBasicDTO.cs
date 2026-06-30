using CommonLibrary.Models;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

public class SubmissionBasicDTO
{
    public int Id { get; set; }
    public required string SubmissionUrl { get; set; }
    public required DateTime SubmittedDate { get; set; }
    public required SubmissionStatus Status { get; set; }
}