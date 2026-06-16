using Azure.Core;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.TaskAssignmentRepository;

namespace traineeManagementAPI.Service.TaskAssignmentService;

public class TaskAssignmentService(ITaskAssignmentRepository repository, ILogger<TaskAssignmentService> logger) : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _repo = repository;
    private readonly ILogger<TaskAssignmentService> _logger = logger;

    public TaskAssignmentResponseDTO MapToTaskAssignmentResponseDTO(TaskAssignment taskAssignment)
    {
        // if (taskAssignment.Trainee == null)
        // {
        //     throw new Exception("Traineee is null");
        // }
        return new TaskAssignmentResponseDTO
        {
            Id = taskAssignment.Id,
            TraineeId = taskAssignment.TraineeId,
            Trainee = new TraineeResponseDTO
            {
                Id = taskAssignment.Trainee.Id,
                FirstName = taskAssignment.Trainee.FirstName,
                LastName = taskAssignment.Trainee.LastName,
                Email = taskAssignment.Trainee.Email,
                TechStack = taskAssignment.Trainee.TechStack,
                Status = taskAssignment.Trainee.Status,
                TaskAssignment = taskAssignment.Trainee.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
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
                CreatedDate = taskAssignment.Trainee.CreatedDate,
                UpdatedDate = taskAssignment.Trainee.UpdatedDate
            },
            MentorId = taskAssignment.MentorId,
            Mentor = new MentorResponseDTO
        {
            Id = taskAssignment.Mentor.Id,
            FirstName = taskAssignment.Mentor.FirstName,
            LastName = taskAssignment.Mentor.LastName,
            Email = taskAssignment.Mentor.Email,
            Expertise = taskAssignment.Mentor.Expertise,
            Status = taskAssignment.Mentor.Status,
            TaskAssignments = taskAssignment.Mentor.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
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
            Reviews = taskAssignment.Mentor.Reviews.Select(r => new ReviewResponseDTO
        {
            Id = r.Id,
            SubmissionId = r.SubmissionId,
            MentorId = r.MentorId,
            Feedback = r.Feedback,
            Score = r.Score ?? null,
            ReviewStatus = r.ReviewStatus,
            ReviewedDate = r.ReviewedDate
        }).ToList(),
            CreatedDate = taskAssignment.Mentor.CreatedDate,
            UpdatedDate = taskAssignment.Mentor.UpdatedDate
        },
            LearningTaskId = taskAssignment.LearningTaskId,
            LearningTask = new LearningTaskResponseDTO
        {
            Id = taskAssignment.LearningTask.Id,
            Title = taskAssignment.LearningTask.Title,
            Description = taskAssignment.LearningTask.Description,
            ExpectedTechStack = taskAssignment.LearningTask.ExpectedTechStack,
            DueDate = taskAssignment.LearningTask.DueDate,
            Status = taskAssignment.LearningTask.Status,
            TaskAssignments = taskAssignment.LearningTask.TaskAssignments.Select(ta => new TaskAssignmentResponseDTO
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
            CreatedDate = taskAssignment.LearningTask.CreatedDate,
            UpdatedDate = taskAssignment.LearningTask.UpdatedDate
        }};
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
            _logger.LogError("Task Assignment with the specified Id is not available.");
            throw new NotFoundException($"Task Assignment with the id - {id} not found");
        }

        return MapToTaskAssignmentResponseDTO(desiredTaskAssignment);

    }

    public async Task<TaskAssignmentResponseDTO> CreateAsync(CreateTaskAssignmentRequestDTO createTaskAssignmentDto)
    {
        TaskAssignment newTaskAssignment = new()
        {
            TraineeId = createTaskAssignmentDto.TraineeId,
            MentorId = createTaskAssignmentDto.MentorId,
            LearningTaskId = createTaskAssignmentDto.LearningTaskId,
            AssignedDate = createTaskAssignmentDto.AssignedDate,
            DueDate = createTaskAssignmentDto.DueDate,
            Status = createTaskAssignmentDto.Status,
            Remarks = createTaskAssignmentDto?.Remarks,
        };

        TaskAssignment CreatedTaskAssignment = await _repo.CreateTaskAssignmentAsync(newTaskAssignment);

        return MapToTaskAssignmentResponseDTO(CreatedTaskAssignment);

    }

    public async Task<TaskAssignmentResponseDTO?> UpdateAsync(int id, UpdateTaskAssignmentRequestDTO updateTaskAssignmentDto)
    {
        var existingTaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (existingTaskAssignment == null)
        {
            _logger.LogError("Task Assignment with the specified Id is not available.");
            throw new NotFoundException($"Task Assignment with the id - {id} not found");
        }
        
        if(updateTaskAssignmentDto.Status != null) 
            existingTaskAssignment.Status = updateTaskAssignmentDto.Status.Value;

        var desiredTrainee = await _repo.UpdateTaskAssignmentAsync(id, existingTaskAssignment);

        if(desiredTrainee == null)
        {
            _logger.LogError("Something went wrong while updating a new Task Assignment.");
            throw new Exception("Something went wrong while updating a new Task Assignment");   
        }

        _logger.LogInformation("Task Assignment Updated Successfully");
        return MapToTaskAssignmentResponseDTO(desiredTrainee);
    }
}