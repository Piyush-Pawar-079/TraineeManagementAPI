using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Model;
using traineeManagementAPI.Helpers;

namespace traineeManagementAPI.Service;

public interface ITraineeService
{
    Task<List<TraineeResponseDTO>> GetAllTrainees();

    Task<TraineeResponseDTO?> GetTraineeById(int id);

    Task<TraineeResponseDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO trainee);

    Task<TraineeResponseDTO> CreateTrainee(CreateTraineeRequestDTO trainee);

    Task<bool> DeleteTrainee(int id);

    Task<List<TraineeResponseDTO>> Search(String searchParam);

    Task<List<TraineeResponseDTO>> Sort(String sortParam, bool ascending);

    Task<List<TraineeResponseDTO>> GetTraineeUsingPagination(PaginationParams paginationParams);

}