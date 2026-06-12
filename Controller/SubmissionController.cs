using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Service.SubmissionService;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("api/submissions")]
public class SubmissionController(ISubmissionService submissionService) : ControllerBase
{
    private readonly ISubmissionService _submissionService = submissionService;

    [HttpGet]
    public async Task<ActionResult> GetAllSubmission()
    {
        var submissions = await _submissionService.GetAllAsync();

        return Ok(submissions);
    }

    [HttpGet("id")]
    public async Task<ActionResult> GetById(int id)
    {
        var submission = await _submissionService.GetByIdAsync(id);
        
        if(submission == null)
            return BadRequest($"Submission with the id {id} is not available");

        return Ok(submission);
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubmission(CreateSubmissionRequestDTO createSubmissionDTO)
    {
        var createdSubmission = await _submissionService.CreateAsync(createSubmissionDTO);

        if(createdSubmission == null)
            return BadRequest($"Something went wrong while creating submission");

        return Ok(createdSubmission);
    }

}