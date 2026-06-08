using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Model;
using traineeManagementAPI.Service;
using traineeManagementAPI.DTO;
using Microsoft.IdentityModel.Tokens;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("/api/trainees")]
public class TraineeController : ControllerBase
{
    private readonly ITraineeService _traineeService;

    public TraineeController(ITraineeService traineeService)
    {
        _traineeService = traineeService;
    }

    [HttpGet]
    public async Task<ActionResult<List<TraineeResponseDTO>>> GetAllTrainees(String? searchParam)
    {

        if (searchParam != null)
        {
            var searchResult = await _traineeService.Search(searchParam);
            if (searchResult.Count == 0)
            {
                return NotFound();
            }
            return Ok(searchResult);
        }

        var response = await _traineeService.GetAllTrainees();
            if (response.Count == 0)
            {
                return NotFound();
            }
            return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TraineeResponseDTO?>> GetTraineeById(int id)
    {
        var trainee = await _traineeService.GetTraineeById(id);

        if (trainee == null)
        {
            return NotFound($"Trainee with Id {id} not found");
        }

        return Ok(trainee);

    }

    [HttpPut]
    public async Task<ActionResult<TraineeResponseDTO?>> UpdateTrainee(int id, UpdateTraineeRequestDTO updateDto)
    {
        var updatedTrainee = await _traineeService.UpdateTrainee(id, updateDto);

        if (updatedTrainee == null)
        {
            return NotFound($"Trainee with Id {id} not found");
        }

        return Ok(updatedTrainee);
    }

    [HttpPost]
    public async Task<ActionResult<TraineeResponseDTO>> CreateTrainee(CreateTraineeRequestDTO createDto)
    {
        return Ok(await _traineeService.CreateTrainee(createDto));
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteTrainee(int id)
    {
        var deleteResult = await _traineeService.DeleteTrainee(id);

        if (deleteResult == false)
        {
            return NotFound($"Trainee with the Id {id} is not available");
        }
        return Ok();

    }

}