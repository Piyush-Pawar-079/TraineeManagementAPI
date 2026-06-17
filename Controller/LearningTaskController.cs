using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.Service.LearningTaskService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/learning-tasks")]
public class LearningTaskController(ILearningTaskService LearningTaskService, ILogger<LearningTaskController> logger) : ControllerBase
{
    private readonly ILearningTaskService _LearningTaskService = LearningTaskService;
    private readonly ILogger<LearningTaskController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<LearningTaskDetailDTO>>> GetAllLearningTasks()
    {
        return await _LearningTaskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningTaskDetailDTO?>> GetLearningTaskById(int id)
    {
        var LearningTask = await _LearningTaskService.GetByIdAsync(id);
        return Ok(LearningTask);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskDetailDTO?>> UpdateLearningTask(int id, UpdateLearningTaskRequestDTO updateDto)
    {
        var updatedLearningTask = await _LearningTaskService.UpdateAsync(id, updateDto);
        return Ok(updatedLearningTask);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTaskDetailDTO>> CreateLearningTask(CreateLearningTaskRequestDTO createDto)
    {
        return Ok(await _LearningTaskService.CreateAsync(createDto));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningTask(int id)
    {
        await _LearningTaskService.DeleteAsync(id);
        return Ok();
    }

}