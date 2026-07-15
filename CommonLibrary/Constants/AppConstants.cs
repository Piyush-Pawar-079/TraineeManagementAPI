namespace CommonLibrary.Constants;

public static class AppConstant
{

    public static readonly string[] AllowedFileExtensions = [".jpg", ".png", ".pdf", ".txt"];

    public static readonly int AllowedFileSize = 5 * 1024 * 1024;

    public static readonly string FileUploadPath = "../CommonLibrary/uploads";

    public static readonly string CorrelationHeader = "X-Correlation-ID";

    public static string TraineeRedisKey(int id)
    {
        return $"Trainee:{id}";
    }
 
    public static string MentorRedisKey(int id)
    {
        return $"Mentor:{id}";
    }
 
    public static string TaskAssignmentRedisKey(int id)
    {
        return $"TaskAssignment:{id}";
    }
 
    public static string SubmissionRedisKey(int id)
    {
        return $"Submission:{id}";
    }
 
    public static string SubmissionSummaryRedisKey(int id)
    {
        return $"SubmissionSummary:{id}";
    }

}