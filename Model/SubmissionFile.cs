namespace traineeManagementAPI.Model;

public class SubmissionFile
{
    public int Id { get; set; }
    public string OriginalFileName { get; set; } = "";
    public string GeneratedFileName { get; set; } = "";
    public string ContentType { get; set; } = "";
    public int Size { get; set; }
    public int CheckSum { get; set; }
    public string OwnerName { get; set; } = "";
    public int SubmissionId { get; set; }
    public Submission Submission { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}