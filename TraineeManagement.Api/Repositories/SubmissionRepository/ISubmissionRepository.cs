using CommonLibrary.Models;

namespace TraineeManagement.Api.Repositories.SubmissionRepository;

public interface ISubmissionRepository
{
    Task<List<Submission>> GetAllSubmissionsAsync();

    Task<Submission?> GetSubmissionByIdAsync(int id);

    Task<Submission> CreateSubmissionAsync(Submission submission);
}