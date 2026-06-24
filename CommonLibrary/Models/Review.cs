namespace CommonLibrary.Models;

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
    public Submission Submission { get; set; } = null!;
    public required int MentorId { get; set; }
    public Mentor Mentor { get; set; } = null!;
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required ReviewStatus ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}