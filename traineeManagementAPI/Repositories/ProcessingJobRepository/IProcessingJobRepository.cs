using CommonLibrary.Models;
using traineeManagementAPI.DTO.ProcessingJobDTOs;

namespace traineeManagementAPI.Repositories.ProcessingJobRepository;

public interface IProcessingJobRepository
{
    Task<ProcessingJob?> GetJobById(int id);
    Task AddJobAsync(ProcessingJob job);
}