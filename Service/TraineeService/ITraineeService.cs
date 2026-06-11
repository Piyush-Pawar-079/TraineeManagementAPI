using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Helpers;
using traineeManagementAPI.DTO.HelperDTOs;

namespace traineeManagementAPI.Service.TraineeService;

public interface ITraineeService
{
    Task<List<TraineeResponseDTO>> GetAllTrainees();

    Task<TraineeResponseDTO?> GetTraineeById(int id);

    Task<TraineeResponseDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO trainee);

    Task<TraineeResponseDTO> CreateTrainee(CreateTraineeRequestDTO trainee);

    Task<bool> DeleteTrainee(int id);
    public Task<List<TraineeResponseDTO>> GetAllAsyncWithFilters(FilterDTO filters, PaginationParams paginationParams);

}