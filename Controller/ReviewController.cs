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
        var Reviews = await _ReviewService.GetAllAsync();

        return Ok(Reviews);
    }

    [HttpGet("id")]
    public async Task<ActionResult> GetById(int id)
    {
        var Review = await _ReviewService.GetByIdAsync(id);
        return Ok(Review);
    }

    [HttpPost]
    public async Task<ActionResult> CreateReview(CreateReviewRequestDTO createReviewDTO)
    {
        var createdReview = await _ReviewService.CreateAsync(createReviewDTO);
        return Ok(createdReview);
    }

}