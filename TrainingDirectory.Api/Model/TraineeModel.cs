namespace TrainingDirectory.Api.Model;
public class TraineeProfile
{
    public int Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Course { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public DateTime JoinedOn { get; set; }

    public int CompletedAssignments { get; set; }
}
