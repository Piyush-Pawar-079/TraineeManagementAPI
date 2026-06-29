using TrainingDirectory.Api.Model;

namespace TrainingDirectory.Api.Data;
public static class DummyTraineeData
{
    public static List<TraineeProfile> Trainees =
    [
        new TraineeProfile
        {
            Id = 1,
            FullName = "Piyush Pawar",
            Email = "piyush@example.com",
            Course = "Backend Engineering",
            Status = "Active",
            JoinedOn = DateTime.UtcNow.AddMonths(-2),
            CompletedAssignments = 5
        },
        new TraineeProfile
        {
            Id = 2,
            FullName = "Rahul Sharma",
            Email = "rahul@example.com",
            Course = "Frontend Engineering",
            Status = "Completed",
            JoinedOn = DateTime.UtcNow.AddMonths(-4),
            CompletedAssignments = 10
        }
    ];
}