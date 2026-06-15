using Azure.Core;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.TaskAssignmentRepository;

namespace traineeManagementAPI.Service.TaskAssignmentService;

public class TaskAssignmentService(ITaskAssignmentRepository repository, ILogger<TaskAssignmentService> logger) : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _repo = repository;
    private readonly ILogger<TaskAssignmentService> _logger = logger;
    private static int _nextId = 0;

    public TaskAssignmentResponseDTO MapToTaskAssignmentResponseDTO(TaskAssignment taskAssignment)
    {
        return new TaskAssignmentResponseDTO
        {
            Id = taskAssignment.Id,
            TraineeId = taskAssignment.TraineeId,
            Trainee = taskAssignment.Trainee,
            MentorId = taskAssignment.MentorId,
            Mentor = taskAssignment.Mentor,
            LearningTaskId = taskAssignment.LearningTaskId,
            LearningTask = taskAssignment.LearningTask,
            Submission = taskAssignment.Submission,
            AssignedDate = taskAssignment.AssignedDate,
            DueDate = taskAssignment.DueDate,
            Status = taskAssignment.Status,
            Remarks = taskAssignment?.Remarks
        };
    }

    public async Task<List<TaskAssignmentResponseDTO>> GetAllAsync()
    {
        var allTaskAssignments = await _repo.GetAllTaskAssignmentAsync();
        return allTaskAssignments.Select(MapToTaskAssignmentResponseDTO).ToList();
    }

    public async Task<TaskAssignmentResponseDTO?> GetByIdAsync(int id)
    {
        var desiredTaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (desiredTaskAssignment == null)
        {
            return null;
        }

        return MapToTaskAssignmentResponseDTO(desiredTaskAssignment);

    }

    public async Task<TaskAssignmentResponseDTO> CreateAsync(CreateTaskAssignmentRequestDTO createTaskAssignmentDto)
    {
        var tasks = await _repo.GetAllTaskAssignmentAsync();

        TaskAssignment newTaskAssignment = new()
        {
            Id = _nextId,
            TraineeId = createTaskAssignmentDto.TraineeId,
            MentorId = createTaskAssignmentDto.MentorId,
            LearningTaskId = createTaskAssignmentDto.LearningTaskId,
            AssignedDate = createTaskAssignmentDto.AssignedDate,
            DueDate = createTaskAssignmentDto.DueDate,
            Status = createTaskAssignmentDto.Status,
            Remarks = createTaskAssignmentDto?.Remarks,
        };

        _nextId++;

        TaskAssignment CreatedTaskAssignment = await _repo.CreateTaskAssignmentAsync(newTaskAssignment);

        return MapToTaskAssignmentResponseDTO(CreatedTaskAssignment);

    }

    public async Task<TaskAssignmentResponseDTO?> UpdateAsync(int id, UpdateTaskAssignmentRequestDTO updateTaskAssignmentDto)
    {
        var existingTaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (existingTaskAssignment == null)
        {
            return null;
        }
        
        if(updateTaskAssignmentDto.Status != null) 
            existingTaskAssignment.Status = updateTaskAssignmentDto.Status;

        var desiredTrainee = await _repo.UpdateTaskAssignmentAsync(id, existingTaskAssignment);

        if(desiredTrainee == null)
            return null;

        _logger.LogInformation("Trainee Updated Successfully");
        return MapToTaskAssignmentResponseDTO(desiredTrainee);
    }
}