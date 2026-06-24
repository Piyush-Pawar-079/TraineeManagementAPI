namespace CommonLibrary.Contract;

public class SubmissionProcessingRequested
{
    public Guid MessageId { get; set; } = Guid.NewGuid();
    public string CorrelationId { get; set; } = "";
    public int SubmissionId { get; set; }
    public string FileId { get; set; } = string.Empty;
    public DateTime RequestedAt { get; set; } = DateTime.UtcNow;
    public string ContractVersion { get; set; } = "1.0.0";
}
