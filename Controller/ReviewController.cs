using Microsoft.AspNetCore.Mvc;
using traineeManagementAPI.DTO.ReviewDTOs;
using traineeManagementAPI.Service.ReviewService;

namespace traineeManagementAPI.Controller;

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
        
        if(Review == null)
            return BadRequest($"Review with the id {id} is not available");

        return Ok(Review);
    }

    [HttpPost]
    public async Task<ActionResult> CreateReview(CreateReviewRequestDTO createReviewDTO)
    {
        var createdReview = await _ReviewService.CreateAsync(createReviewDTO);

        if(createdReview == null)
            return BadRequest($"Something went wrong while creating Review");

        return Ok(createdReview);
    }

}