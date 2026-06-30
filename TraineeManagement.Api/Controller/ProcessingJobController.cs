using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.Service.ProcessingJobService;

namespace TraineeManagement.Api.Controller;

[ApiController]
[Route("api/processing-jobs")]
public class ProcessingJobsController(IProcessingJobService jobService) : ControllerBase
{
    private readonly IProcessingJobService _jobService = jobService;

    [HttpGet("{id}")]
    public async Task<IActionResult> GetJobState(int id)
    {
        return Ok(await _jobService.GetProcessingJobById(id));
    }
}
