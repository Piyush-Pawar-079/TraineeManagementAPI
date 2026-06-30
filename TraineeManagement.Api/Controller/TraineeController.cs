using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Service.TraineeService;
using TraineeManagement.Api.DTO.TraineeDTOs;
using TraineeManagement.Api.Helpers;
using TraineeManagement.Api.DTO.HelperDTOs;
using Microsoft.AspNetCore.Authorization;

namespace TraineeManagement.Api.Controller;

[Authorize]
[ApiController]
[Route("/api/trainees")]
public class TraineeController(ITraineeService traineeService) : ControllerBase
{
    private readonly ITraineeService _traineeService = traineeService;

    [HttpGet]
    public async Task<ActionResult<List<TraineeDetailDTO>>> GetAllTrainees([FromQuery] FilterDTO filters, [FromQuery] PaginationParams paginationParams)
    {
        return Ok(await _traineeService.GetAllAsyncWithFilters(filters, paginationParams));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeDetailDTO?>> GetTraineeById(int id)
    {
        return Ok(await _traineeService.GetTraineeById(id));

    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TraineeDetailDTO?>> UpdateTrainee(int id, UpdateTraineeRequestDTO updateDto)
    {
        return Ok(await _traineeService.UpdateTrainee(id, updateDto));
    }

    [HttpPost]
    public async Task<ActionResult<TraineeDetailDTO>> CreateTrainee(CreateTraineeRequestDTO createDto)
    {
        var trainee = await _traineeService.CreateTrainee(createDto);
        return CreatedAtAction(nameof(GetTraineeById), new { id = trainee.Id }, trainee);
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        await _traineeService.DeleteTrainee(id);
        return NoContent();
    }

}