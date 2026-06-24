using Microsoft.EntityFrameworkCore;
using CommonLibrary.Models;

namespace CommonLibrary.Data;

public class ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : DbContext(options)
{
    public DbSet<Trainee> Trainees { set; get; }

    public DbSet<User> Users {set; get;}

    public DbSet<Mentor> Mentors { set; get; }

    public DbSet<LearningTask> LearningTasks { set; get; }

    public DbSet<TaskAssignment> TaskAssignments { set; get; }

    public DbSet<Submission> Submissions { get; set; }

    public DbSet<Review> Reviews { get; set; }

    public DbSet<SubmissionFile> SubmissionFiles { get; set; }

    public DbSet<ProcessingJob> ProcessingJobs { get; set; }

    public DbSet<ProcessedMessage> ProcessedMessages { get; set; }

     protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.Trainee)          // Each TaskAssignment has one Trainee
            .WithMany(t => t.TaskAssignments)        // Each Trainee has many TaskAssignments
            .HasForeignKey(ta => ta.TraineeId); // Foreign key in TaskAssignment table

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.Mentor)          // Each TaskAssignment has one Mentor
            .WithMany(m => m.TaskAssignments)        // Each Mentor has many TaskAssignments
            .HasForeignKey(ta => ta.MentorId); // Foreign key in TaskAssignment table

        modelBuilder.Entity<TaskAssignment>()
            .HasOne(ta => ta.LearningTask)          // Each TaskAssignemnt has one LearningTask
            .WithMany(t => t.TaskAssignments)        // Each LearningTask has many TaskAssignments
            .HasForeignKey(ta => ta.LearningTaskId); // Foreign key in Task Assignment table

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

        modelBuilder.Entity<SubmissionFile>()
            .HasOne(sf => sf.Submission)
            .WithMany(s => s.SubmissionFiles)
            .HasForeignKey(sf => sf.SubmissionId);

        modelBuilder.Entity<ProcessingJob>()
            .HasIndex(j => j.CorrelationId);
            
        modelBuilder.Entity<ProcessingJob>()
            .HasIndex(j => j.FileId);
        
    }

}