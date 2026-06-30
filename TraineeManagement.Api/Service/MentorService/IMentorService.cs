using TraineeManagement.Api.DTO.MentorDTOs;

namespace TraineeManagement.Api.Service.MentorService;

public interface IMentorService
{
    Task<List<MentorDetailDTO>> GetAllAsync();

    Task<MentorDetailDTO?> GetByIdAsync(int id);

    Task<MentorDetailDTO> CreateAsync(CreateMentorRequestDTO createMentorDto);

    Task<MentorDetailDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync();
}