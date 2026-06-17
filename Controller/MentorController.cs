using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.Service.MentorService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/Mentors")]
public class MentorController(IMentorService mentorService, ILogger<MentorController> logger) : ControllerBase
{
    private readonly IMentorService _mentorService = mentorService;
    private readonly ILogger<MentorController> _logger = logger;

    [HttpGet]
    public async Task<ActionResult<List<MentorDetailDTO>>> GetAllMentors()
    {
        return await _mentorService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MentorDetailDTO?>> GetMentorById(int id)
    {
        var mentor = await _mentorService.GetByIdAsync(id);
        return Ok(mentor);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MentorDetailDTO?>> UpdateMentor(int id, UpdateMentorRequestDTO updateDto)
    {
        var updatedMentor = await _mentorService.UpdateAsync(id, updateDto);
        return Ok(updatedMentor);
    }

    [HttpPost]
    public async Task<ActionResult<MentorDetailDTO>> CreateMentor(CreateMentorRequestDTO createDto)
    {
        return Ok(await _mentorService.CreateAsync(createDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        await _mentorService.DeleteAsync(id);
        return Ok();
    }

}