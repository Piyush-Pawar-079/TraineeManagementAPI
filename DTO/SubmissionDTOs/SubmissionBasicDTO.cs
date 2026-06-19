using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.SubmissionDTOs;

public class SubmissionBasicDTO
{
    public int Id { get; set; }
    public required SubmissionStatus Status { get; set; }
}