namespace traineeManagementAPI.Model;

public class Submission
{
    public int Id { get; set; } // auto-generated
    public required int TaskAssignmentId { get; set; } 
    public required String SubmissionUrl { get; set; }
    public required DateTime SubmittedDate { get; set; }
    public required String Status { get; set; }
}