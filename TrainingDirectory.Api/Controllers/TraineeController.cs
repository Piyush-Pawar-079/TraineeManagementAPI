using Microsoft.AspNetCore.Mvc;
using TrainingDirectory.Api.Data;
using TrainingDirectory.Api.DTO;

namespace TrainingDirectory.Api.Controllers
{
    [ApiController]
    [Route("api/trainees")]
    public class TraineesController(ILogger<TraineesController> logger) : ControllerBase
    {
        private readonly ILogger<TraineesController> _logger = logger;

        [HttpGet("{id}&{correlationId}")]
        public ActionResult<TraineeProfileResponseDto> GetTraineeById(int id, string correlationId)
        {
            var trainee = DummyTraineeData.Trainees.FirstOrDefault(t => t.Id == id);

            if (trainee == null)
            {
                _logger.LogError("Trainees data not found. CorrelationId: {CorrelationId}", correlationId);
                return NotFound();
            }

            // Map Model → DTO
            TraineeProfileResponseDto response = new()
            {
                Id = trainee.Id,
                Name = trainee.FullName,
                Course = trainee.Course,
                Status = trainee.Status,
                CompletedAssignments = trainee.CompletedAssignments
            };
            _logger.LogInformation("Sending the trainee with the Id: {id}. CorrelationId: {CorrelationId}", id, correlationId);
            return Ok(response);
        }

        [HttpGet("{correlationId}")]
        public ActionResult<TraineeProfileResponseDto> GetTrainee(string correlationId)
        {
            var trainee = DummyTraineeData.Trainees;

            if (trainee == null)
            {
                _logger.LogError("Something went wrong while getting the trainees data. CorrelationId: {CorrelationId}", correlationId);
                return NotFound();
            }
            // Map Model → DTO
            TraineeProfileResponseDto response = (TraineeProfileResponseDto)trainee.Select(t => new TraineeProfileResponseDto
            {
                Id = t.Id,
                Name = t.FullName,
                Course = t.Course,
                Status = t.Status,
                CompletedAssignments = t.CompletedAssignments
            });

            _logger.LogInformation("Sending all the trainees data. CorrelationID: {CorrelationId}", correlationId);

            return Ok(response);
        }
    }
}