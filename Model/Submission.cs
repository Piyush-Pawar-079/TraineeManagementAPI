namespace traineeManagementAPI.Model;

public enum SubmissionStatus
{
    Submitted, 
    Resubmitted
}

public class Submission
{
    public int Id { get; set; } // auto-generated
    public required int TaskAssignmentId { get; set; } 
    public TaskAssignment? TaskAssignment { get; set; } 
    public required string SubmissionUrl { get; set; }
    public required string Notes { get; set; }
    public required DateTime SubmittedDate { get; set; }
    public required string Status { get; set; }
}