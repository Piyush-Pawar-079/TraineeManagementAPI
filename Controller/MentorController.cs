using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.Service.MentorService;

namespace traineeManagementAPI.Controller;

[ApiController]
[Route("/api/Mentors")]
public class MentorController(IMentorService mentorService, ILogger<MentorController> logger) : ControllerBase
{
    private readonly IMentorService _mentorService = mentorService;
    private readonly ILogger<MentorController> _logger = logger;

    // [Authorize]
    [HttpGet]
    public async Task<ActionResult<List<MentorResponseDTO>>> GetAllMentors()
    {
        return await _mentorService.GetAllAsync();
    }

    // [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<MentorResponseDTO?>> GetMentorById(int id)
    {
        var mentor = await _mentorService.GetByIdAsync(id);

        if (mentor == null)
        {
            _logger.LogError("mentor with the specified Id is not available.");
            return NotFound($"mentor with Id {id} not found");
        }

        return Ok(mentor);

    }

    // [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult<MentorResponseDTO?>> UpdateMentor(int id, UpdateMentorRequestDTO updateDto)
    {
        var updatedMentor = await _mentorService.UpdateAsync(id, updateDto);

        if (updatedMentor == null)
        {
            _logger.LogError("Mentor with the specified Id is not available to update.");
            return NotFound($"Mentor with Id {id} not found");
        }

        return Ok(updatedMentor);
    }

    // [Authorize]
    [HttpPost]
    public async Task<ActionResult<MentorResponseDTO>> CreateMentor(CreateMentorRequestDTO createDto)
    {
        return Ok(await _mentorService.CreateAsync(createDto));
    }


    // [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMentor(int id)
    {
        var deleteResult = await _mentorService.DeleteAsync(id);

        if (deleteResult == false)
        {
            _logger.LogError("Mentor with the specified Id is not available to delete.");
            return NotFound($"Mentor with the Id {id} is not available");
        }
        return Ok();

    }

}