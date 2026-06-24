using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Service.ProcessingJobService;

namespace traineeManagementAPI.Controller;

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
