using CommonLibrary.Models;

namespace traineeManagementAPI.Repositories.UserRepository;

public interface IUserRepository
{
    Task<List<User>> GetAllAsync();

    Task<User?> GetByIdAsync(int id);

    Task<User?> UpdateAsync(int id, User user);

    Task<User> CreateAsync(User user);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 
}