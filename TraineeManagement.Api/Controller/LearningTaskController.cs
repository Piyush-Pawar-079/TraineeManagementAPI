using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.LearningTaskDTOs;
using TraineeManagement.Api.Service.LearningTaskService;

namespace TraineeManagement.Api.Controller;

[Authorize]
[ApiController]
[Route("/api/learning-tasks")]
public class LearningTaskController(ILearningTaskService LearningTaskService) : ControllerBase
{
    private readonly ILearningTaskService _LearningTaskService = LearningTaskService;

    [HttpGet]
    public async Task<ActionResult<List<LearningTaskDetailDTO>>> GetAllLearningTasks()
    {
        return Ok(await _LearningTaskService.GetAllAsync());
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
        // return Ok(await _LearningTaskService.CreateAsync(createDto));
        var LearningTask = await _LearningTaskService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetLearningTaskById), new { id = LearningTask.Id }, LearningTask);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningTask(int id)
    {
        await _LearningTaskService.DeleteAsync(id);
        return NoContent();
    }

}