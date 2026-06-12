using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.Service.LearningTaskService;

namespace traineeManagementAPI.Controller;

// [Authorize]
[ApiController]
[Route("/api/learning-tasks")]
public class LearningTaskController(ILearningTaskService LearningTaskService, ILogger<LearningTaskController> logger) : ControllerBase
{
    private readonly ILearningTaskService _LearningTaskService = LearningTaskService;
    private readonly ILogger<LearningTaskController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<LearningTaskResponseDTO>>> GetAllLearningTasks()
    {
        return await _LearningTaskService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LearningTaskResponseDTO?>> GetLearningTaskById(int id)
    {
        var LearningTask = await _LearningTaskService.GetByIdAsync(id);

        if (LearningTask == null)
        {
            _logger.LogError("LearningTask with the specified Id is not available.");
            return NotFound($"LearningTask with Id {id} not found");
        }

        return Ok(LearningTask);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<LearningTaskResponseDTO?>> UpdateLearningTask(int id, UpdateLearningTaskRequestDTO updateDto)
    {
        var updatedLearningTask = await _LearningTaskService.UpdateAsync(id, updateDto);

        if (updatedLearningTask == null)
        {
            _logger.LogError("LearningTask with the specified Id is not available to update.");
            return NotFound($"LearningTask with Id {id} not found");
        }

        return Ok(updatedLearningTask);
    }

    [HttpPost]
    public async Task<ActionResult<LearningTaskResponseDTO>> CreateLearningTask(CreateLearningTaskRequestDTO createDto)
    {
        return Ok(await _LearningTaskService.CreateAsync(createDto));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLearningTask(int id)
    {
        var deleteResult = await _LearningTaskService.DeleteAsync(id);

        if (deleteResult == false)
        {
            _logger.LogError("LearningTask with the specified Id is not available to delete.");
            return NotFound($"LearningTask with the Id {id} is not available");
        }
        return Ok();

    }

}