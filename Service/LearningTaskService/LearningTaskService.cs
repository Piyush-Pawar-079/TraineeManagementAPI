using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.LearningTaskRepository;

namespace traineeManagementAPI.Service.LearningTaskService;

public class LearningTaskService(ILearningTaskRepository repository, ILogger<LearningTaskService> logger) : ILearningTaskService
{
    private readonly ILearningTaskRepository _repo = repository;
    private readonly ILogger<LearningTaskService> _logger = logger;
    private static int _nextId = 0;

    private LearningTaskResponseDTO MapToLearningTaskResponseDTO(LearningTask LearningTask)
    {
        return new LearningTaskResponseDTO
        {
            Id = LearningTask.Id,
            Title = LearningTask.Title,
            Description = LearningTask.Description,
            ExpectedTechStack = LearningTask.ExpectedTechStack,
            DueDate = LearningTask.DueDate,
            Status = LearningTask.Status,
            CreatedDate = LearningTask.CreatedDate,
            UpdatedDate = LearningTask.UpdatedDate
        };
    }

    public async Task<List<LearningTaskResponseDTO>> GetAllAsync()
    {
        var allLearningTasks = await _repo.GetAllAsync();
        return allLearningTasks.Select(MapToLearningTaskResponseDTO).ToList();
    }

    public async Task<LearningTaskResponseDTO?> GetByIdAsync(int id)
    {
        var desiredLearningTask = await _repo.GetByIdAsync(id);

        if (desiredLearningTask == null)
        {
            return null;
        }

        return MapToLearningTaskResponseDTO(desiredLearningTask);

    }

    public async Task<LearningTaskResponseDTO> CreateAsync(CreateLearningTaskRequestDTO createLearningTaskDto)
    {
        LearningTask newLearningTask = new()
        {
            Id = _nextId,
            Title = createLearningTaskDto.Title,
            Description = createLearningTaskDto.Description,
            ExpectedTechStack = createLearningTaskDto.ExpectedTechStack,
            DueDate = createLearningTaskDto.DueDate,
            Status = createLearningTaskDto.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _nextId++;

        LearningTask CreatedLearningTask = await _repo.CreateAsync(newLearningTask);

        return MapToLearningTaskResponseDTO(CreatedLearningTask);

    }

    public async Task<LearningTaskResponseDTO?> UpdateAsync(int id, UpdateLearningTaskRequestDTO updateLearningTaskDto)
    {
        var existingLearningTask = await _repo.GetByIdAsync(id);

        if (existingLearningTask == null)
        {
            return null;
        }

        if(updateLearningTaskDto.Title != null) 
            existingLearningTask.Title = updateLearningTaskDto.Title;
        
        if(updateLearningTaskDto.Description != null) 
            existingLearningTask.Description = updateLearningTaskDto.Description;

        if(updateLearningTaskDto.ExpectedTechStack != null) 
            existingLearningTask.ExpectedTechStack = updateLearningTaskDto.ExpectedTechStack;
        
        if(updateLearningTaskDto.DueDate != null) 
            existingLearningTask.DueDate = updateLearningTaskDto.DueDate.Value;
        
        if(updateLearningTaskDto.Status != null) 
            existingLearningTask.Status = updateLearningTaskDto.Status;

        existingLearningTask.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repo.UpdateAsync(id, existingLearningTask);

        if(desiredTrainee == null)
            return null;

        _logger.LogInformation("Trainee Updated Successfully");
        return MapToLearningTaskResponseDTO(desiredTrainee);
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