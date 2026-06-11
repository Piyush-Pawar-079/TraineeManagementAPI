using traineeManagementAPI.DTO.MentorDTOs;

namespace traineeManagementAPI.Service.MentorService;

interface IMentorService
{
    Task<List<MentorResponseDTO>> GetAllAsync();

    Task<MentorResponseDTO?> GetByIdAsync(int id);

    Task<MentorResponseDTO> CreateAsync(CreateMentorRequestDTO createMentorDto);

    Task<MentorResponseDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 
}