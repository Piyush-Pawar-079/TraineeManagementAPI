using AutoMapper;
using CommonLibrary.Models;
using TraineeManagement.Api.DTO.TraineeDTOs;
using TraineeManagement.Api.DTO.MentorDTOs;
using TraineeManagement.Api.DTO.LearningTaskDTOs;
using TraineeManagement.Api.DTO.TaskAssignmentDTOs;
using TraineeManagement.Api.DTO.SubmissionDTOs;
using TraineeManagement.Api.DTO.ReviewDTOs;
using TraineeManagement.Api.DTO.UserDTOs;
using TraineeManagement.Api.DTO.SubmissionFileDTOs;
using TraineeManagement.Api.DTO.ProcessingJobDTOs;

namespace TraineeManagement.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Trainee, TraineeBasicDTO>();
        CreateMap<Trainee, TraineeDetailDTO>();

        CreateMap<User, UserResponseDTO>();
        CreateMap<CreateUserRequestDTO, User>();
        CreateMap<User, CreateUserRequestDTO>();

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

        CreateMap<SubmissionFile, SubmissionFileResponseDTO>();

        CreateMap<ProcessingJob, ProcessingJobResponseDTO>();

    }
}