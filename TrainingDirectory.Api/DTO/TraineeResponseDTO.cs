namespace TrainingDirectory.Api.DTO;

public class TraineeProfileResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Course { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public int CompletedAssignments { get; set; }
}