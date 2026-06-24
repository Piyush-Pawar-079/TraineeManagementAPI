using traineeManagementAPI.Helpers;
using CommonLibrary.Models;

namespace traineeManagementAPI.Repositories.TraineeRepository;

public interface ITraineeRepository
{
    Task<List<Trainee>> GetAllAsync();

    Task<Trainee?> GetByIdAsync(int id);

    Task<Trainee?> UpdateAsync(int id, Trainee trainee);

    Task<Trainee> CreateAsync(Trainee trainee);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 

    Task<PagedResponse<Trainee>> PaginatedResponse(PaginationParams paginationParams, List<Trainee> trainees);

}