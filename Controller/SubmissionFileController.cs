using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.Service.FileStorageService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/submission-files")]
public class SubmissionFileController(IFileStorageService fileStorageService) : ControllerBase
{

    private readonly IFileStorageService _fileStorageService = fileStorageService;

    [HttpGet]
    public async Task<bool> FileExists(int id)
    {
        return await _fileStorageService.ExistsAsync(id);
    }

    [HttpGet("{id}/download")]
    public async Task<IActionResult> DownloadFile(int id)
    {
        var (bytes, contentType, fileName) = await _fileStorageService.OpenReadAsync(id);
        return File(bytes, contentType, fileName);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFile(int id)
    {
        await _fileStorageService.DeleteAsync(id);
        return NoContent();
    }

}