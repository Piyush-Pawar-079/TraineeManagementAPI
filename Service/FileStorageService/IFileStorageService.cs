using traineeManagementAPI.DTO.SubmissionFileDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Service.FileStorageService;

public interface IFileStorageService
{
    Task<SubmissionFileResponseDTO> SaveAsync(int submissionId, CreateSubmissionFileDTO createDTO, CancellationToken cancellationToken);

    Task<(byte[] bytes, string contentType, string fileName)> OpenReadAsync(int id);

    Task<bool> ExistsAsync(int id);

    Task DeleteAsync(int id);
}