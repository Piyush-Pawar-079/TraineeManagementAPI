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

        // ✅ GET TRAINEE BY ID
        [HttpGet("{id}")]
        public ActionResult<TraineeProfileResponseDto> GetTraineeById(int id)
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            var trainee = DummyTraineeData.Trainees.FirstOrDefault(t => t.Id == id);

            if (trainee == null)
            {
                _logger.LogError("Trainee not found. CorrelationId: {CorrelationId}", correlationId);
                return NotFound();
            }

            var response = new TraineeProfileResponseDto
            {
                Id = trainee.Id,
                Name = trainee.FullName,
                Course = trainee.Course,
                Status = trainee.Status,
                CompletedAssignments = trainee.CompletedAssignments
            };

            _logger.LogInformation("Returning trainee {Id}. CorrelationId: {CorrelationId}", id, correlationId);

            return Ok(response);
        }

        // ✅ GET ALL TRAINEES
        [HttpGet]
        public ActionResult<List<TraineeProfileResponseDto>> GetTrainees()
        {
            var correlationId = HttpContext.Request.Headers["X-Correlation-ID"].FirstOrDefault();

            var response = DummyTraineeData.Trainees.Select(t => new TraineeProfileResponseDto
            {
                Id = t.Id,
                Name = t.FullName,
                Course = t.Course,
                Status = t.Status,
                CompletedAssignments = t.CompletedAssignments
            }).ToList();

            _logger.LogInformation("Returning all trainees. CorrelationId: {CorrelationId}", correlationId);

            return Ok(response);
        }
    }
}