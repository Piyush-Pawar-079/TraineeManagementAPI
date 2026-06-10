using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Service;
using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Helpers;

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
    public async Task<ActionResult<List<TraineeResponseDTO>>> GetAllTrainees(String? searchParam, String? statusFilter, String? sortParam, bool? ascending, [FromQuery] PaginationParams paginationParams)
    {
        // var finalResponse = new List<Trainee>();

        if (paginationParams != null)
        {
            var paginatedResponse = await _traineeService.GetTraineeUsingPagination(paginationParams);
            if (paginatedResponse.Count == 0)
            {
                return NotFound();
            }
            if (searchParam == null && statusFilter == null)
            {
                return Ok(paginatedResponse);
            }
            // finalResponse = paginatedResponse;
        }

        if (sortParam != null)
        {
            var sortedResult = await _traineeService.Sort(sortParam, ascending != false);
            if (sortedResult.Count == 0)
            {
                return NotFound();
            }
            return Ok(sortedResult);
        }

        if (searchParam != null)
        {
            // var searchResult = await _traineeService.Search(searchParam, finalResponse);
            var searchResult = await _traineeService.Search(searchParam);
            if (searchResult.Count == 0)
            {
                return NotFound();
            }
            
            if(statusFilter == null)
                return Ok(searchResult);

            // finalResponse = searchResult;
        }

        // if (statusFilter != null)   
        // {
        //     // var searchResult = await _traineeService.filterByStatus(searchParam, finalResponse);
        //     var searchResult = await _traineeService.filterByStatus
        //     if (searchResult.Count == 0)
        //     {
        //         return NotFound();
        //     }
            
        //     if(statusFilter == null)
        //         return Ok(searchResult);

        //     finalResponse = searchResult;
        // }

        // if(searchParam == null && statusFilter == null && paginationParams != null)
        //     finalResponse = await _traineeService.GetAllTrainees();
        
        // if (finalResponse.Count == 0)
        // {
        //     return NotFound();
        // }
        // return Ok(finalResponse);
        return await _traineeService.GetAllTrainees();
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

    [HttpPut("{id}")]
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

    [HttpDelete("{id}")]
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