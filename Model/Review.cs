namespace traineeManagementAPI.Model;

public class Review
{
    public int Id { get; set; } // auto-generated
    public required int SubmissionId { get; set; } 
    public required int MentorId { get; set; }
    public required string Feedback { get; set; }
    public int? Score { get; set; }
    public required string ReviewStatus { get; set; }
    public DateTime ReviewedDate { get; set; } 
}