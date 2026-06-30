using CommonLibrary.Models;

namespace TraineeManagement.Api.Repositories.ProcessingJobRepository;

public interface IProcessingJobRepository
{
    Task<ProcessingJob?> GetJobById(int id);
    Task AddJobAsync(ProcessingJob job);
}