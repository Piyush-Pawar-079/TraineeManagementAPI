using System.ComponentModel.DataAnnotations;

namespace CommonLibrary.Models;

public class ProcessedMessage
{
    [Key]
    public Guid MessageId { get; set; }
    public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
}