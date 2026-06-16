using System.Text.Json.Serialization;

namespace traineeManagementAPI.Model;

public enum ReviewStatus
{
    Accepted,
    ChangesRequired,
    Rejected
}


public class Review
{
    public int Id { get; set; } // auto-generated
    public required int SubmissionId { get; set; } 

    [JsonIgnore]
    public Submission? Submission { get; set; }
    public required int MentorId { get; set; }
    
    [JsonIgnore]
    public Mentor? Mentor { get; set; }
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required ReviewStatus ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}