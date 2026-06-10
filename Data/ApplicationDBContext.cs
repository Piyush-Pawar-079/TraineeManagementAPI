using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Data;

public class ApplicationDBContext : DbContext
{
    
    public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
    {
        
    }

    public DbSet<Trainee> Trainees { set; get; }

    public DbSet<User> Users {set; get;}

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Admin", Email = "admin@gmail.com", PasswordHash = "admin@12345", Role = "Admin", CreatedDate = DateTime.Now, UpdatedDate = DateTime.Now }
        );
    }

}