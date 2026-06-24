using CommonLibrary.Models;

namespace traineeManagementAPI.DTO.ReviewDTOs;

public class ReviewBasicDTO
{
    public int Id { get; set; }
    public string Feedback { get; set; } = string.Empty;
    public int? Score { get; set; }
    public ReviewStatus ReviewStatus { get; set; }
}