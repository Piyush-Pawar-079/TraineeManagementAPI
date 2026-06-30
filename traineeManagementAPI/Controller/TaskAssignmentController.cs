using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Service.TaskAssignmentService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/task-assignment")]
public class TaskAssignmentController(ITaskAssignmentService TaskAssignmentService) : ControllerBase
{
    private readonly ITaskAssignmentService _taskAssignmentService = TaskAssignmentService;

    [HttpGet]
    public async Task<ActionResult<List<TaskAssignmentDetailDTO>>> GetAllTaskAssignments()
    {
        return await _taskAssignmentService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskAssignmentDetailDTO?>> GetTaskAssignmentById(int id)
    {
        return Ok(await _taskAssignmentService.GetByIdAsync(id));

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskAssignmentDetailDTO?>> UpdateTaskAssignment(int id, UpdateTaskAssignmentRequestDTO updateDto)
    {
        return Ok(await _taskAssignmentService.UpdateAsync(id, updateDto));
    }

    [HttpPost]
    public async Task<ActionResult<TaskAssignmentDetailDTO>> CreateTaskAssignment(CreateTaskAssignmentRequestDTO createDto)
    {
        // return Ok(await _taskAssignmentService.CreateAsync(createDto));
        var TaskAssignment = await _taskAssignmentService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetTaskAssignmentById), new { id = TaskAssignment.Id }, TaskAssignment);
    }

}