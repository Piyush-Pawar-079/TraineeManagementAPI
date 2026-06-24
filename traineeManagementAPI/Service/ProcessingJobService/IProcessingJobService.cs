using traineeManagementAPI.DTO.ProcessingJobDTOs;

namespace traineeManagementAPI.Service.ProcessingJobService;

public interface IProcessingJobService
{
    Task<ProcessingJobResponseDTO> GetProcessingJobById(int id);
}