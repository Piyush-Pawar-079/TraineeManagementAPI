using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Model;
using traineeManagementAPI.Service;
using traineeManagementAPI.DTO;

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
    public IActionResult GetAllTrainees()
    {
        var trainee = _traineeService.GetAllTrainees();

        var response = trainee.Select(trainee => new TraineeResponseDTO
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
            CreateDate = trainee.CreatedDate,
            UpdateDate = trainee.UpdatedDate
        });

        return Ok(response);

    }

    [HttpGet("{id}")]
    public IActionResult GetTraineeById(int id)
    {
        var trainee = _traineeService.GetTraineeById(id);

        if (trainee == null)
        {
            return NotFound("Trainee with desired Id is not found");
        }

        return Ok(new TraineeResponseDTO
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
            CreateDate = trainee.CreatedDate,
            UpdateDate = trainee.UpdatedDate
        });

    }

    [HttpPut]
    public IActionResult UpdateTrainee(int id, UpdateTraineeRequestDTO trainee)
    {
        var desiredTrainee = _traineeService.GetTraineeById(id);

        if (desiredTrainee == null)
        {
            return NotFound("Trainee with the specified id is not available");
        }

        if (trainee.FirstName != null)
        {
            desiredTrainee.FirstName = trainee.FirstName;
        }

        if (trainee.LastName != null)
        {
            desiredTrainee.LastName = trainee.LastName;
        }

        if (trainee.Email != null)
        {
            desiredTrainee.Email = trainee.Email;
        }

        if (trainee.TechStack != null)
        {
            desiredTrainee.TechStack = trainee.TechStack;
        }

        if (trainee.Status != null)
        {
            desiredTrainee.Status = trainee.Status;
        }

        desiredTrainee.UpdatedDate = DateTime.Now;

        return Ok(new TraineeResponseDTO
        {
            Id = desiredTrainee.Id,
            FirstName = desiredTrainee.FirstName,
            LastName = desiredTrainee.LastName,
            Email = desiredTrainee.Email,
            TechStack = desiredTrainee.TechStack,
            Status = desiredTrainee.Status,
            CreateDate = desiredTrainee.CreatedDate,
            UpdateDate = desiredTrainee.UpdatedDate
        });

    }

    [HttpPost]
    public IActionResult CreateTrainee(CreateTraineeRequestDTO trainee)
    {
        var newTrainee = new Trainee
        {
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
        };

        var createdTrainee = _traineeService.CreateTrainee(newTrainee);

        return Ok(new TraineeResponseDTO
        {
            Id = createdTrainee.Id,
            FirstName = createdTrainee.FirstName,
            LastName = createdTrainee.LastName,
            Email = createdTrainee.Email,
            TechStack = createdTrainee.TechStack,
            Status = createdTrainee.Status,
            CreateDate = createdTrainee.CreatedDate,
            UpdateDate = createdTrainee.UpdatedDate
        });

    }

    [HttpDelete]
    public IActionResult DeleteTrainee(int id)
    {
        var trainee = _traineeService.DeleteTrainee(id);

        if (!trainee)
        {
            return NotFound("Trainee with desired Id is not found");
        }

        return NoContent();

    }

}