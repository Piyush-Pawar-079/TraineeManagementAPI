using traineeManagementAPI.DTO;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Service;

public interface ITraineeService
{
    Task<List<TraineeResponseDTO>> GetAllTrainees();

    Task<TraineeResponseDTO?> GetTraineeById(int id);

    Task<TraineeResponseDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO trainee);

    Task<TraineeResponseDTO> CreateTrainee(CreateTraineeRequestDTO trainee);

    Task<bool> DeleteTrainee(int id);

    Task<List<TraineeResponseDTO>> Search(String searchParam);

    Task<List<TraineeResponseDTO>> Sort(String sortParam);

}