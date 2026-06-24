using AutoMapper;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using CommonLibrary.Models;
using traineeManagementAPI.Repositories.LearningTaskRepository;

namespace traineeManagementAPI.Service.LearningTaskService;

public class LearningTaskService(ILearningTaskRepository repository, ILogger<LearningTaskService> logger, IMapper mapper) : ILearningTaskService
{
    private readonly ILearningTaskRepository _repo = repository;
    private readonly ILogger<LearningTaskService> _logger = logger;
    private readonly IMapper _mapper = mapper;

    public async Task<List<LearningTaskDetailDTO>> GetAllAsync()
    {
        var allLearningTasks = await _repo.GetAllAsync();
        return _mapper.Map<List<LearningTaskDetailDTO>>(allLearningTasks);
    }

    public async Task<LearningTaskDetailDTO?> GetByIdAsync(int id)
    {
        var desiredLearningTask = await _repo.GetByIdAsync(id);

        if (desiredLearningTask == null)
        {
            _logger.LogError("LearningTask with the specified Id is not available.");
            throw new NotFoundException($"Learning Task with the id - {id} not found");
        }

        return _mapper.Map<LearningTaskDetailDTO>(desiredLearningTask);

    }

    public async Task<LearningTaskDetailDTO> CreateAsync(CreateLearningTaskRequestDTO createLearningTaskDto)
    {
        // LearningTask newLearningTask = new()
        // {
        //     Title = createLearningTaskDto.Title,
        //     Description = createLearningTaskDto.Description,
        //     ExpectedTechStack = createLearningTaskDto.ExpectedTechStack,
        //     DueDate = createLearningTaskDto.DueDate,
        //     Status = createLearningTaskDto.Status,
        //     CreatedDate = DateTime.UtcNow,
        //     UpdatedDate = DateTime.UtcNow
        // };

        LearningTask newLearningTask = _mapper.Map<LearningTask>(createLearningTaskDto);

        LearningTask CreatedLearningTask = await _repo.CreateAsync(newLearningTask);

        return _mapper.Map<LearningTaskDetailDTO>(CreatedLearningTask);

    }

    public async Task<LearningTaskDetailDTO?> UpdateAsync(int id, UpdateLearningTaskRequestDTO updateLearningTaskDto)
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
        return _mapper.Map<LearningTaskDetailDTO>(desiredTrainee);
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