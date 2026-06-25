using Microsoft.AspNetCore.Mvc;
using TrainingDirectory.Api.Data;
using TrainingDirectory.Api.DTO;

namespace TrainingDirectory.Api.Controllers
{
    [ApiController]
    [Route("api/trainees")]
    public class TraineesController : ControllerBase
    {
        [HttpGet("{id}")]
        public ActionResult<TraineeProfileResponseDto> GetTrainee(int id)
        {
            var trainee = DummyTraineeData.Trainees.FirstOrDefault(t => t.Id == id);

            if (trainee == null)
                return NotFound();

            // Map Model → DTO
            var response = new TraineeProfileResponseDto
            {
                Id = trainee.Id,
                Name = trainee.FullName,
                Course = trainee.Course,
                Status = trainee.Status,
                CompletedAssignments = trainee.CompletedAssignments
            };

            return Ok(response);
        }

        [HttpGet]
        public ActionResult<TraineeProfileResponseDto> GetTrainee()
        {
            var trainee = DummyTraineeData.Trainees;

            if (trainee == null)
                return NotFound();

            // Map Model → DTO
            var response = trainee.Select(t => new TraineeProfileResponseDto
            {
                Id = t.Id,
                Name = t.FullName,
                Course = t.Course,
                Status = t.Status,
                CompletedAssignments = t.CompletedAssignments
            });

            return Ok(response);
        }
    }
}