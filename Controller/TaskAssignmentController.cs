using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Service.TaskAssignmentService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/task-assignment")]
public class TaskAssignmentController(ITaskAssignmentService TaskAssignmentService, ILogger<TaskAssignmentController> logger) : ControllerBase
{
    private readonly ITaskAssignmentService _taskAssignmentService = TaskAssignmentService;
    private readonly ILogger<TaskAssignmentController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<TaskAssignmentDetailDTO>>> GetAllTaskAssignments()
    {
        return await _taskAssignmentService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAssignmentDetailDTO?>> GetTaskAssignmentById(int id)
    {
        var TaskAssignment = await _taskAssignmentService.GetByIdAsync(id);
        return Ok(TaskAssignment);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskAssignmentDetailDTO?>> UpdateTaskAssignment(int id, UpdateTaskAssignmentRequestDTO updateDto)
    {
        var updatedTaskAssignment = await _taskAssignmentService.UpdateAsync(id, updateDto);
        return Ok(updatedTaskAssignment);
    }

    [HttpPost]
    public async Task<ActionResult<TaskAssignmentDetailDTO>> CreateTaskAssignment(CreateTaskAssignmentRequestDTO createDto)
    {
        return Ok(await _taskAssignmentService.CreateAsync(createDto));
    }

}