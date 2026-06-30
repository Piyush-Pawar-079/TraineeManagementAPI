using CommonLibrary.Models;

namespace TraineeManagement.Api.Repositories.SubmissionFileRepository;

public interface ISubmissionFileRepository
{
    Task<SubmissionFile> SaveAsync(SubmissionFile submissionFile);

    Task<SubmissionFile?> GetSubmissionFileById(int id);

    Task DeleteAsync(SubmissionFile submissionFile);
}