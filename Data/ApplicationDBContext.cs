using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.DTO.UserDTOs;
using traineeManagementAPI.Model;

namespace traineeManagementAPI.Data;

public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
{
    public DbSet<Trainee> Trainees { set; get; }

    public DbSet<User> Users {set; get;}

    public DbSet<Mentor> Mentors { set; get; }

    public DbSet<LearningTask> LearningTasks { set; get; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Admin", Email = "admin@gmail.com", PasswordHash = "admin@12345", Role = Role.Admin.ToString(), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
        );
    }

}