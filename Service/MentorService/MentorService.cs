using Azure.Core;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.MentorRepository;

namespace traineeManagementAPI.Service.MentorService;

public class MentorService(IMentorRepository repository, ILogger<MentorService> logger) : IMentorService
{
    private readonly IMentorRepository _repo = repository;
    private readonly ILogger<MentorService> _logger = logger;

    public MentorResponseDTO MapToMentorResponseDTO(Mentor mentor)
    {
        return new MentorResponseDTO
        {
            Id = mentor.Id,
            FirstName = mentor.FirstName,
            LastName = mentor.LastName,
            Email = mentor.Email,
            Expertise = mentor.Expertise,
            Status = mentor.Status,
            TaskAssignments = mentor.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
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
            Reviews = mentor.Reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            SubmissionId = r.SubmissionId,
            MentorId = r.MentorId,
            Feedback = r.Feedback,
            Score = r.Score ?? null,
            ReviewStatus = r.ReviewStatus,
            ReviewedDate = r.ReviewedDate
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
            _logger.LogError("Mentor with the specified Id is not available.");
            throw new NotFoundException($"Mentor with the id - {id} not found");
        }

        return MapToMentorResponseDTO(desiredMentor);

    }

    public async Task<MentorResponseDTO> CreateAsync(CreateMentorRequestDTO createMentorDto)
    {
        Mentor newMentor = new()
        {
            FirstName = createMentorDto.FirstName,
            LastName = createMentorDto.LastName,
            Email = createMentorDto.Email,
            Expertise = createMentorDto.Expertise,
            Status = createMentorDto.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        Mentor CreatedMentor = await _repo.CreateAsync(newMentor);

        return MapToMentorResponseDTO(CreatedMentor);

    }

    public async Task<MentorResponseDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto)
    {
        var existingMentor = await _repo.GetByIdAsync(id);

        if (existingMentor == null)
        {
            _logger.LogError("Mentor with the specified Id is not available.");
            throw new NotFoundException($"Mentor with the id - {id} not found");
        }

        if(updateMentorDto.FirstName != null) 
            existingMentor.FirstName = updateMentorDto.FirstName;
        
        if(updateMentorDto.LastName != null) 
            existingMentor.LastName = updateMentorDto.LastName;

        if(updateMentorDto.Email != null) 
            existingMentor.Email = updateMentorDto.Email;
        
        if(updateMentorDto.Expertise != null) 
            existingMentor.Expertise = updateMentorDto.Expertise;
        
        if(updateMentorDto.Status.HasValue) 
            existingMentor.Status = updateMentorDto.Status.Value;

        existingMentor.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repo.UpdateAsync(id, existingMentor);

        if(desiredTrainee == null)
        {
            _logger.LogError("Something went wrong while updating a new Mentor.");
            throw new Exception("Something went wrong while updating a new Mentor");
        }

        _logger.LogInformation("Mentor Updated Successfully");
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