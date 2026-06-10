
using traineeManagementAPI.Helpers;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories;

public interface ITraineeRepository
{
    Task<List<Trainee>> GetAllAsync();

    Task<Trainee?> GetByIdAsync(int id);

    Task<Trainee?> UpdateAsync(int id, Trainee trainee);

    Task<Trainee> CreateAsync(Trainee trainee);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 

    Task<PagedResponse<Trainee>> PaginatedResponse(PaginationParams paginationParams);

}