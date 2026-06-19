using traineeManagementAPI.DTO.SubmissionFileDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.SubmissionFileRepository;

public interface ISubmissionFileRepository
{
    Task<SubmissionFile> SaveAsync(SubmissionFile submissionFile);

    Task<SubmissionFile?> GetSubmissionFileById(int id);

    Task DeleteAsync(SubmissionFile submissionFile);
}