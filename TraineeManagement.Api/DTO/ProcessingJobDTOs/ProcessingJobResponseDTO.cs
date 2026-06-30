using System.ComponentModel.DataAnnotations;
using CommonLibrary.Models;

namespace TraineeManagement.Api.DTO.ProcessingJobDTOs;

public class ProcessingJobResponseDTO
{
    [Key]
    public int Id { get; set; }
    public String CorrelationId { get; set; } = "";
    public int SubmissionId { get; set; }
    public int FileId { get; set; }
    public JobStatus Status { get; set; }
    public int Attempts { get; set; }
    public string? ErrorSummary { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? StartedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}