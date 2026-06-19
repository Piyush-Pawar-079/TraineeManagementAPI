using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.Service.ReviewService;

namespace traineeManagementAPI.Controller;

[Authorize]
[ApiController]
[Route("api/reviews")]
public class ReviewController(IReviewService ReviewService) : ControllerBase
{
    private readonly IReviewService _ReviewService = ReviewService;

    [HttpGet]
    public async Task<ActionResult> GetAllReview()
    {
        return Ok(await _ReviewService.GetAllAsync());
    }

    [HttpGet("id")]
    public async Task<ActionResult> GetById(int id)
    {
        return Ok(await _ReviewService.GetByIdAsync(id));
    }

    [HttpPost]
    public async Task<ActionResult> CreateReview(CreateReviewRequestDTO createReviewDTO)
    {
        return Ok(await _ReviewService.CreateAsync(createReviewDTO));
    }

}