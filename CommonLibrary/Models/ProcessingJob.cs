namespace CommonLibrary.Models;

public enum JobStatus
{
    Queued,
    Processing,
    Completed,
    Failed
}

public class ProcessingJob
{
    public int Id { get; set; }
    public string CorrelationId { get; set; } = "";
    public int SubmissionId { get; set; }
    public int FileId { get; set; }
    public JobStatus Status { get; set; }
    public int Attempts { get; set; }
    public string? ErrorSummary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}