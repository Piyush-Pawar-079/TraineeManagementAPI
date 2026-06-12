using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.SubmissionRepository;

public interface ISubmissionRepository
{
    Task<List<Submission>> GetAllSubmissionsAsync();

    Task<Submission?> GetSubmissionByIdAsync(int id);

    Task<Submission> CreateSubmissionAsync(Submission submission);
}