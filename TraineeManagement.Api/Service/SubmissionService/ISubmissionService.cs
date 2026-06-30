using TraineeManagement.Api.DTO.SubmissionDTOs;

namespace TraineeManagement.Api.Service.SubmissionService;

public interface ISubmissionService
{
    Task<List<SubmissionDetailDTO>> GetAllAsync();

    Task<SubmissionDetailDTO?> GetByIdAsync(int id);

    Task<SubmissionBasicDTO> GetSummary(int id);

    Task<SubmissionDetailDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto);
}