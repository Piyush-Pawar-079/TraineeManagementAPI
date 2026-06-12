using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Repositories.MentorRepository;

public class MentorRepository(ApplicationDBContext context) : IMentorRepository
{
    private readonly ApplicationDBContext _context = context;

    public async Task<List<Mentor>> GetAllAsync()
    {
        return await _context.Mentors.Include(m => m.TaskAssignments).ToListAsync();
    }

    public async Task<Mentor?> GetByIdAsync(int id)
    {
        return await _context.Mentors.Include(m => m.TaskAssignments).FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<Mentor> CreateAsync(Mentor mentor)
    {
        await _context.Mentors.AddAsync(mentor);
        await _context.SaveChangesAsync();

        return mentor;
    }

    public async Task<Mentor?> UpdateAsync(int id, Mentor mentor)
    {
        _context.Mentors.Update(mentor);
        await _context.SaveChangesAsync();

        return await _context.Mentors.FindAsync(id);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var mentor = await _context.Mentors.FindAsync(id);
        if (mentor == null)
        {
            return false;
        }
        _context.Mentors.Remove(mentor);
        await _context.SaveChangesAsync();

        return true;
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

}