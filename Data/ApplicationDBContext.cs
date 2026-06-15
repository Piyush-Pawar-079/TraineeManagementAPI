using Microsoft.AspNetCore.Mvc;
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

    public DbSet<TaskAssignment> TaskAssignments { set; get; }

    public DbSet<Submission> Submissions { get; set; }

    public DbSet<Review> Reviews { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed initial data
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "Admin", Email = "admin@gmail.com", PasswordHash = "admin@12345", Role = Role.Admin.ToString(), CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow }
        );

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.Trainee)          // Each TaskAssignment has one Trainee
            .WithMany(t => t.TaskAssignments)        // Each Trainee has many TaskAssignments
            .HasForeignKey(ta => ta.TraineeId) // Foreign key in TaskAssignment table
            .OnDelete(DeleteBehavior.Cascade); // Cascade Null

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.Mentor)          // Each TaskAssignment has one Mentor
            .WithMany(m => m.TaskAssignments)        // Each Mentor has many TaskAssignments
            .HasForeignKey(ta => ta.MentorId) // Foreign key in TaskAssignment table
            .OnDelete(DeleteBehavior.Cascade); // Cascade Null

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.LearningTask)          // Each TaskAssignemnt has one LearningTask
            .WithMany(t => t.TaskAssignments)        // Each LearningTask has many TaskAssignments
            .HasForeignKey(ta => ta.LearningTaskId) // Foreign key in Task Assignment table
            .OnDelete(DeleteBehavior.Cascade); // Cascade Null

        modelBuilder.Entity<Submission>()
            .HasOne(s => s.TaskAssignment)
            .WithMany(ta => ta.Submission)
            .HasForeignKey(s => s.TaskAssignmentId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Submission)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.SubmissionId);

        modelBuilder.Entity<Review>()
            .HasOne(r => r.Mentor)
            .WithMany(s => s.Reviews)
            .HasForeignKey(r => r.MentorId);
        
    }

}