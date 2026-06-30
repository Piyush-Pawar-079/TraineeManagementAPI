using traineeManagementAPI.DTO.SubmissionDTOs;

namespace traineeManagementAPI.Service.SubmissionService;

public interface ISubmissionService
{
    Task<List<SubmissionDetailDTO>> GetAllAsync();

    Task<SubmissionDetailDTO?> GetByIdAsync(int id);

    Task<SubmissionBasicDTO> GetSummary(int id);

    Task<SubmissionDetailDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto);
}