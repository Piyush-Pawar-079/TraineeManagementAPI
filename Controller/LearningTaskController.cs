using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.Service.LearningTaskService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/learning-tasks")]
public class LearningTaskController(ILearningTaskService LearningTaskService) : ControllerBase
{
    private readonly ILearningTaskService _LearningTaskService = LearningTaskService;

    [HttpGet]
    public async Task<ActionResult<List<LearningTaskDetailDTO>>> GetAllLearningTasks()
    {
        return await _LearningTaskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningTaskDetailDTO?>> GetLearningTaskById(int id)
    {
        return Ok(await _LearningTaskService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskDetailDTO?>> UpdateLearningTask(int id, UpdateLearningTaskRequestDTO updateDto)
    {
        return Ok(await _LearningTaskService.UpdateAsync(id, updateDto));
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