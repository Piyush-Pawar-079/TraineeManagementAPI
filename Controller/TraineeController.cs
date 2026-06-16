using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Service.TraineeService;
using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Helpers;
using traineeManagementAPI.DTO.HelperDTOs;
using Microsoft.AspNetCore.Authorization;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/trainees")]
public class TraineeController(ITraineeService traineeService, ILogger<TraineeController> logger) : ControllerBase
{
    private readonly ITraineeService _traineeService = traineeService;
    private readonly ILogger<TraineeController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<TraineeResponseDTO>>> GetAllTrainees([FromQuery] FilterDTO filters, [FromQuery]PaginationParams paginationParams)
    {
        return await _traineeService.GetAllAsyncWithFilters(filters, paginationParams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeResponseDTO?>> GetTraineeById(int id)
    {
        var trainee = await _traineeService.GetTraineeById(id);

        if (trainee == null)
        {
            _logger.LogError("Trainee with the specified Id is not available.");
            return NotFound($"Trainee with Id {id} not found");
        }

        return Ok(trainee);

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeResponseDTO?>> UpdateTrainee(int id, UpdateTraineeRequestDTO updateDto)
    {
        
        var updatedTrainee = await _traineeService.UpdateTrainee(id, updateDto);

        if (updatedTrainee == null)
        {
            _logger.LogError("Trainee with the specified Id is not available to update.");
            return NotFound($"Trainee with Id {id} not found");
        }

        return Ok(updatedTrainee);
    }

    [HttpPost]
    public async Task<ActionResult<TraineeResponseDTO>> CreateTrainee(CreateTraineeRequestDTO createDto)
    {
        Console.WriteLine(createDto.Status);
        Console.WriteLine(createDto.Status.GetType());
        _logger.LogError(createDto.Status.ToString());
        return Ok(await _traineeService.CreateTrainee(createDto));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        var deleteResult = await _traineeService.DeleteTrainee(id);

        if (deleteResult == false)
        {
            _logger.LogError("Trainee with the specified Id is not available to delete.");
            return NotFound($"Trainee with the Id {id} is not available");
        }
        return Ok();

    }

}