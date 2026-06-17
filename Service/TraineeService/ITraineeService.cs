using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Helpers;
using traineeManagementAPI.DTO.HelperDTOs;

namespace traineeManagementAPI.Service.TraineeService;

public interface ITraineeService
{
    Task<List<TraineeDetailDTO>> GetAllTrainees();

    Task<TraineeDetailDTO?> GetTraineeById(int id);

    Task<TraineeDetailDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO trainee);

    Task<TraineeDetailDTO> CreateTrainee(CreateTraineeRequestDTO trainee);

    Task<bool> DeleteTrainee(int id);
    public Task<List<TraineeDetailDTO>> GetAllAsyncWithFilters(FilterDTO filters, PaginationParams paginationParams);

}