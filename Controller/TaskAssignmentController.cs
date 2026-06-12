using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Service.TaskAssignmentService;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("/api/task-assignment")]
public class TaskAssignmentController(ITaskAssignmentService TaskAssignmentService, ILogger<TaskAssignmentController> logger) : ControllerBase
{
    private readonly ITaskAssignmentService _taskAssignmentService = TaskAssignmentService;
    private readonly ILogger<TaskAssignmentController> _logger = logger;

    // [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<TaskAssignmentResponseDTO>>> GetAllTaskAssignments()
    {
        return await _taskAssignmentService.GetAllAsync();
    }

    // [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAssignmentResponseDTO?>> GetTaskAssignmentById(int id)
    {
        var TaskAssignment = await _taskAssignmentService.GetByIdAsync(id);

        if (TaskAssignment == null)
        {
            _logger.LogError("TaskAssignment with the specified Id is not available.");
            return NotFound($"TaskAssignment with Id {id} not found");
        }

        return Ok(TaskAssignment);

    }

    // [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<TaskAssignmentResponseDTO?>> UpdateTaskAssignment(int id, UpdateTaskAssignmentRequestDTO updateDto)
    {
        var updatedTaskAssignment = await _taskAssignmentService.UpdateAsync(id, updateDto);

        if (updatedTaskAssignment == null)
        {
            _logger.LogError("TaskAssignment with the specified Id is not available to update.");
            return NotFound($"TaskAssignment with Id {id} not found");
        }

        return Ok(updatedTaskAssignment);
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<TaskAssignmentResponseDTO>> CreateTaskAssignment(CreateTaskAssignmentRequestDTO createDto)
    {
        return Ok(await _taskAssignmentService.CreateAsync(createDto));
    }

}