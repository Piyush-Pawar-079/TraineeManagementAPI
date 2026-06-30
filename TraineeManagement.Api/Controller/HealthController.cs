using Microsoft.AspNetCore.Mvc;

namespace TraineeManagement.Api.Controller;

[ApiController]
[Route("/api/health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IResult GetHealthStatus()
    {
        return Results.Ok(new
        {
            status = "running",
            application = "Trainee Management API",
            timestamp = DateTime.UtcNow
        });
    }
}