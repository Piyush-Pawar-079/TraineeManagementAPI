using CommonLibrary.Models;
using TraineeManagement.Api.DTO.ProcessingJobDTOs;

namespace TraineeManagement.Api.Service.ProcessingJobService;

public interface IProcessingJobService
{
    Task<ProcessingJobResponseDTO> GetProcessingJobById(int id);

    Task AddProcessingJob(ProcessingJob job);
}