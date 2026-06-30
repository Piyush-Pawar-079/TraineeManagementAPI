using TraineeManagement.Api.DTO.TraineeDTOs;
using TraineeManagement.Api.Helpers;
using TraineeManagement.Api.DTO.HelperDTOs;

namespace TraineeManagement.Api.Service.TraineeService;

public interface ITraineeService
{
    Task<List<TraineeDetailDTO>> GetAllTrainees();

    Task<TraineeDetailDTO?> GetTraineeById(int id);

    Task<TraineeDetailDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO trainee);

    Task<TraineeDetailDTO> CreateTrainee(CreateTraineeRequestDTO trainee);

    Task<bool> DeleteTrainee(int id);
    public Task<List<TraineeDetailDTO>> GetAllAsyncWithFilters(FilterDTO filters, PaginationParams paginationParams);

}