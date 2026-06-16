using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.LearningTaskRepository;

namespace traineeManagementAPI.Service.LearningTaskService;

public class LearningTaskService(ILearningTaskRepository repository, ILogger<LearningTaskService> logger) : ILearningTaskService
{
    private readonly ILearningTaskRepository _repo = repository;
    private readonly ILogger<LearningTaskService> _logger = logger;

    public LearningTaskResponseDTO MapToLearningTaskResponseDTO(LearningTask LearningTask)
    {
        return new LearningTaskResponseDTO
        {
            Id = LearningTask.Id,
            Title = LearningTask.Title,
            Description = LearningTask.Description,
            ExpectedTechStack = LearningTask.ExpectedTechStack,
            DueDate = LearningTask.DueDate,
            Status = LearningTask.Status,
            TaskAssignments = LearningTask.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
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
            _logger.LogError("LearningTask with the specified Id is not available.");
            throw new NotFoundException($"Learning Task with the id - {id} not found");
        }

        return MapToLearningTaskResponseDTO(desiredLearningTask);

    }

    public async Task<LearningTaskResponseDTO> CreateAsync(CreateLearningTaskRequestDTO createLearningTaskDto)
    {
        LearningTask newLearningTask = new()
        {
            Title = createLearningTaskDto.Title,
            Description = createLearningTaskDto.Description,
            ExpectedTechStack = createLearningTaskDto.ExpectedTechStack,
            DueDate = createLearningTaskDto.DueDate,
            Status = createLearningTaskDto.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        LearningTask CreatedLearningTask = await _repo.CreateAsync(newLearningTask);

        return MapToLearningTaskResponseDTO(CreatedLearningTask);

    }

    public async Task<LearningTaskResponseDTO?> UpdateAsync(int id, UpdateLearningTaskRequestDTO updateLearningTaskDto)
    {
        var existingLearningTask = await _repo.GetByIdAsync(id);

        if (existingLearningTask == null)
        {
            _logger.LogError("LearningTask with the specified Id is not available.");
            throw new NotFoundException($"Learning Task with the id - {id} not found");
        }

        if(updateLearningTaskDto.Title != null) 
            existingLearningTask.Title = updateLearningTaskDto.Title;
        
        if(updateLearningTaskDto.Description != null) 
            existingLearningTask.Description = updateLearningTaskDto.Description;

        if(updateLearningTaskDto.ExpectedTechStack != null) 
            existingLearningTask.ExpectedTechStack = updateLearningTaskDto.ExpectedTechStack;
        
        if(updateLearningTaskDto.DueDate != null) 
            existingLearningTask.DueDate = updateLearningTaskDto.DueDate.Value;
        
        if(updateLearningTaskDto.Status.HasValue) 
            existingLearningTask.Status = updateLearningTaskDto.Status.Value;

        existingLearningTask.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repo.UpdateAsync(id, existingLearningTask);

        if(desiredTrainee == null)
        {
            _logger.LogError("Something went wrong while updating a new Learning Task.");
            throw new Exception("Something went wrong while updating a new Learning Task");
        }
        _logger.LogInformation("Learning Task Updated Successfully");
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