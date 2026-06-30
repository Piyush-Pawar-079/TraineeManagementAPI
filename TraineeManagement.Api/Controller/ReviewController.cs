using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TraineeManagement.Api.DTO.ReviewDTOs;
using TraineeManagement.Api.Service.ReviewService;

namespace TraineeManagement.Api.Controller;

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
        // return Ok(await _ReviewService.CreateAsync(createReviewDTO));
        var Review = await _ReviewService.CreateAsync(createReviewDTO);
        return CreatedAtAction(nameof(GetById), new { id = Review.Id }, Review);
    }

}