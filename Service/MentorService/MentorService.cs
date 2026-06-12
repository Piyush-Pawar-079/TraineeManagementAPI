using Azure.Core;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.MentorRepository;

namespace traineeManagementAPI.Service.MentorService;

public class MentorService(IMentorRepository repository, ILogger<MentorService> logger) : IMentorService
{
    private readonly IMentorRepository _repo = repository;
    private readonly ILogger<MentorService> _logger = logger;
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
            TaskAssignmentId = mentor.TaskAssignments.Select(ta => new TaskAssignment
            {
                Id = ta.Id,
                TraineeId = ta.TraineeId,
                MentorId = ta.MentorId,
                LearningTaskId = ta.LearningTaskId,
                AssignedDate = ta.AssignedDate,
                DueDate = ta.DueDate,
                Status = ta.Status,
                Remarks = ta?.Remarks
            }).ToList(),
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
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _nextId++;

        Mentor CreatedMentor = await _repo.CreateAsync(newMentor);

        return MapToMentorResponseDTO(CreatedMentor);

    }

    public async Task<MentorResponseDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto)
    {
        var existingMentor = await _repo.GetByIdAsync(id);

        if (existingMentor == null)
        {
            return null;
        }

        if(updateMentorDto.FirstName != null) 
            existingMentor.FirstName = updateMentorDto.FirstName;
        
        if(updateMentorDto.LastName != null) 
            existingMentor.LastName = updateMentorDto.LastName;

        if(updateMentorDto.Email != null) 
            existingMentor.Email = updateMentorDto.Email;
        
        if(updateMentorDto.Expertise != null) 
            existingMentor.Expertise = updateMentorDto.Expertise;
        
        if(updateMentorDto.Status != null) 
            existingMentor.Status = updateMentorDto.Status;

        existingMentor.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repo.UpdateAsync(id, existingMentor);

        if(desiredTrainee == null)
            return null;

        _logger.LogInformation("Trainee Updated Successfully");
        return MapToMentorResponseDTO(desiredTrainee);
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