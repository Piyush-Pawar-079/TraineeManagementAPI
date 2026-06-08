using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Data;

public class ApplicationDBContext : DbContext
{
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        
    }

    public DbSet<Trainee> Trainees { set; get; }

}