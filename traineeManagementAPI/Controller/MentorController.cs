using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.Service.MentorService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("/api/Mentors")]
public class MentorController(IMentorService mentorService) : ControllerBase
{
    private readonly IMentorService _mentorService = mentorService;

    [HttpGet]
    public async Task<ActionResult<List<MentorDetailDTO>>> GetAllMentors()
    {
        return await _mentorService.GetAllAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MentorDetailDTO?>> GetMentorById(int id)
    {
        return Ok(await _mentorService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MentorDetailDTO?>> UpdateMentor(int id, UpdateMentorRequestDTO updateDto)
    {
        return Ok(await _mentorService.UpdateAsync(id, updateDto));
    }

    [HttpPost]
    public async Task<ActionResult<MentorDetailDTO>> CreateMentor(CreateMentorRequestDTO createDto)
    {
        // return Ok(await _mentorService.CreateAsync(createDto));
        var Mentor = await _mentorService.CreateAsync(createDto);
        return CreatedAtAction(nameof(GetMentorById), new {id = Mentor.Id }, Mentor);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        await _mentorService.DeleteAsync(id);
        return NoContent();
    }

}