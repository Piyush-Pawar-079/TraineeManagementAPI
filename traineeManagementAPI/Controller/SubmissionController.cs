using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.SubmissionFileDTOs;
using traineeManagementAPI.Service.FileStorageService;
using traineeManagementAPI.Service.SubmissionService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("api/submissions")]
public class SubmissionController(ISubmissionService submissionService, IFileStorageService fileStorageService) : ControllerBase
{
    private readonly ISubmissionService _submissionService = submissionService;
    private readonly IFileStorageService _fileStorageService = fileStorageService;

    [HttpGet]
    public async Task<ActionResult> GetAllSubmission()
    {
        return Ok(await _submissionService.GetAllAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await _submissionService.GetByIdAsync(id));
    }

    [HttpGet("{id}/summary")]
    public async Task<ActionResult> GetSubmissionSummary(int id)
    {
        return Ok(await _submissionService.GetSummary(id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateSubmission(CreateSubmissionRequestDTO createSubmissionDTO)
    {
        // return Ok(await _submissionService.CreateAsync(createSubmissionDTO));
        var Submission = await _submissionService.CreateAsync(createSubmissionDTO);
        return CreatedAtAction(nameof(GetById), new { id = Submission.Id }, Submission);
    }

    [DisableRequestSizeLimit]
    [HttpPost("{submissionId}/files")]
    public async Task<ActionResult> UploadSubmissionFile(int submissionId, CreateSubmissionFileDTO createDTO, CancellationToken cancellationToken)
    {
        // return Ok(await _fileStorageService.SaveAsync(submissionId, createDTO, cancellationToken));
        await _fileStorageService.SaveAsync(submissionId, createDTO, cancellationToken);
        return Accepted();
    }
}