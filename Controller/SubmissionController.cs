using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Service.SubmissionService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("api/submissions")]
public class SubmissionController(ISubmissionService submissionService) : ControllerBase
{
    private readonly ISubmissionService _submissionService = submissionService;

    [HttpGet]
    public async Task<ActionResult> GetAllSubmission()
    {
        return Ok(await _submissionService.GetAllAsync());
    }

    [HttpGet("id")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await _submissionService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubmission(CreateSubmissionRequestDTO createSubmissionDTO)
    {
        return Ok(await _submissionService.CreateAsync(createSubmissionDTO));
    }

}