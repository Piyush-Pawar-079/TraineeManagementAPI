using Microsoft.EntityFrameworkCore;
using CommonLibrary.Models;
using CommonLibrary.Data;

namespace TraineeManagement.Api.Repositories.UserRepository;

public class UserRepository(ApplicationDBContext context) : IUserRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<User>> GetAllAsync()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<User?> UpdateAsync(int id, User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
        return await _context.Users.FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<User> CreateAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var desiredUser = await _context.Users.FindAsync(id);
        if (desiredUser == null)
        {
            return false;
        }
        _context.Users.Remove(desiredUser);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}