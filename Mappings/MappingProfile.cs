using AutoMapper;
using traineeManagementAPI.Model;
using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.DTO.LearningTaskDTOs;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.DTO.ReviewDTOs;

namespace traineeManagementAPI.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Trainee, TraineeBasicDTO>();
        CreateMap<Trainee, TraineeDetailDTO>();

        CreateMap<Mentor, MentorBasicDTO>();
        CreateMap<Mentor, MentorDetailDTO>();

        CreateMap<CreateTaskAssignmentRequestDTO, TaskAssignment>();

        CreateMap<CreateTraineeRequestDTO, Trainee>();

        CreateMap<CreateSubmissionRequestDTO, SubmissionDetailDTO>();

        CreateMap<CreateReviewRequestDTO, Review>();

        CreateMap<CreateMentorRequestDTO, Mentor>();

        CreateMap<CreateLearningTaskRequestDTO, LearningTask>();

        CreateMap<LearningTask, LearningTaskBasicDTO>();
        CreateMap<LearningTask, LearningTaskDetailDTO>();

        CreateMap<TaskAssignment, TaskAssignmentBasicDTO>()
            .ForMember(dest => dest.TraineeName,
                opt => opt.MapFrom(src => src.Trainee.FirstName + " " + src.Trainee.LastName))
            .ForMember(dest => dest.MentorName,
                opt => opt.MapFrom(src => src.Mentor.FirstName))
            .ForMember(dest => dest.LearningTaskTitle,
                opt => opt.MapFrom(src => src.LearningTask.Title));

        CreateMap<TaskAssignment, TaskAssignmentDetailDTO>(); 

        CreateMap<Submission, SubmissionBasicDTO>();
        CreateMap<Submission, SubmissionDetailDTO>();

        CreateMap<Review, ReviewBasicDTO>();
        CreateMap<Review, ReviewDetailDTO>();
    }
}