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
public class TraineeController(ITraineeService traineeService) : ControllerBase
{
    private readonly ITraineeService _traineeService = traineeService;

    [HttpGet]
    public async Task<ActionResult<List<TraineeDetailDTO>>> GetAllTrainees([FromQuery] FilterDTO filters, [FromQuery]PaginationParams paginationParams)
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
        return Ok(await _traineeService.CreateTrainee(createDto));
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        await _traineeService.DeleteTrainee(id);
        return Ok();

    }

}