using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.MentorRepository;

public interface IMentorRepository
{
    Task<List<Mentor>> GetAllAsync();

    Task<Mentor?> GetByIdAsync(int id);

    Task<Mentor> CreateAsync(Mentor mentor);

    Task<Mentor?> UpdateAsync(int id, Mentor mentor);

    Task<bool> DeleteAsync(int id);

    Task SaveChangesAsync(); 
}