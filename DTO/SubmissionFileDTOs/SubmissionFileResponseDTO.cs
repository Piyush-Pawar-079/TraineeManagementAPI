namespace traineeManagementAPI.DTO.SubmissionFileDTOs;

public class SubmissionFileResponseDTO
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = "";
    public string ContentType { get; set; } = "";
    public long Size { get; set; }
    public string OwnerName { get; set; } = "";
    public int SubmissionId { get; set; }
    public DateTime CreatedAt { get; set; }
}