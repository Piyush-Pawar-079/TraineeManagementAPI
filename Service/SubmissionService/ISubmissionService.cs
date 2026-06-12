using traineeManagementAPI.DTO.SubmissionDTOs;

namespace traineeManagementAPI.Service.SubmissionService;

public interface ISubmissionService
{
    Task<List<SubmissionResponseDTO>> GetAllAsync();

    Task<SubmissionResponseDTO?> GetByIdAsync(int id);

    Task<SubmissionResponseDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto);
}