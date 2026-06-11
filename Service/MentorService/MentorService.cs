using Azure.Core;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.MentorRepository;

namespace traineeManagementAPI.Service.MentorService;

public class MentorService(IMentorRepository repository) : IMentorService
{
    private readonly IMentorRepository _repo = repository;
    private static int _nextId = 0;

    private MentorResponseDTO MapToMentorResponseDTO(Mentor mentor)
    {
        return new MentorResponseDTO
        {
            Id = mentor.Id,
            FirstName = mentor.FirstName,
            LastName = mentor.LastName,
            Email = mentor.Email,
            Expertise = mentor.Expertise,
            Status = mentor.Status,
            CreatedDate = mentor.CreatedDate,
            UpdatedDate = mentor.UpdatedDate
        };
    }

    public async Task<List<MentorResponseDTO>> GetAllAsync()
    {
        var allMentors = await _repo.GetAllAsync();
        return allMentors.Select(MapToMentorResponseDTO).ToList();
    }

    public async Task<MentorResponseDTO?> GetByIdAsync(int id)
    {
        var desiredMentor = await _repo.GetByIdAsync(id);

        if (desiredMentor == null)
        {
            return null;
        }

        return MapToMentorResponseDTO(desiredMentor);

    }

    public async Task<MentorResponseDTO> CreateAsync(CreateMentorRequestDTO createMentorDto)
    {
        Mentor newMentor = new()
        {
            Id = _nextId,
            FirstName = createMentorDto.FirstName,
            LastName = createMentorDto.LastName,
            Email = createMentorDto.Email,
            Expertise = createMentorDto.Expertise,
            Status = createMentorDto.Status,
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        _nextId++;

        Mentor CreatedMentor = await _repo.CreateAsync(newMentor);

        return MapToMentorResponseDTO(CreatedMentor);

    }

    public async Task<MentorResponseDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto)
    {
        // var updatedMentor = await _repo.UpdateAsync(id, updateMentorDto);
        return null;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        return await _repo.DeleteAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _repo.SaveChangesAsync();
    }

}