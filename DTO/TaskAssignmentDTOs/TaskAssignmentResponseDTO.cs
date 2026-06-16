using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.DTO.TaskAssignmentDTOs;

public class TaskAssignmentResponseDTO
{
    public int Id { get; set; } // auto-generated
    public required int TraineeId { get; set; }
    public TraineeResponseDTO? Trainee { get; set; }
    public required int MentorId { get; set; }
    public MentorResponseDTO? Mentor { get; set; }
    public required int LearningTaskId { get; set; }
    public LearningTaskResponseDTO? LearningTask { get; set; } 
    public List<SubmissionResponseDTO> Submission { get; set; } = [];
    public DateTime AssignedDate { get; set; }
    public DateTime DueDate { get; set; }
    public TaskAssigmentStatus Status { get; set; }
    public string? Remarks { get; set; }
}