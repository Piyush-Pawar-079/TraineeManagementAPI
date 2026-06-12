using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;

using traineeManagementAPI.Repositories.LearningTaskRepository;
using traineeManagementAPI.Repositories.MentorRepository;
using traineeManagementAPI.Repositories.ReviewRepository;
using traineeManagementAPI.Repositories.SubmissionRepository;
using traineeManagementAPI.Repositories.TaskAssignmentRepository;
using traineeManagementAPI.Repositories.TraineeRepository;
using traineeManagementAPI.Repositories.UserRepository;

using traineeManagementAPI.Service.AuthService;
using traineeManagementAPI.Service.LearningTaskService;
using traineeManagementAPI.Service.MentorService;
using traineeManagementAPI.Service.ReviewService;
using traineeManagementAPI.Service.SubmissionService;
using traineeManagementAPI.Service.TaskAssignmentService;
using traineeManagementAPI.Service.TraineeService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
   options.AddPolicy("AllowSpecificOrigin",
      policy =>
      {
         policy.WithOrigins("http://localhost:5173")
               .AllowAnyHeader()
               .AllowAnyMethod();
      }
   );
});

builder.Services.AddControllers()
   .AddJsonOptions(options =>
   {
      options.JsonSerializerOptions.Converters.Add(
         new JsonStringEnumConverter()
      );
   });

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);


builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<ITraineeService, TraineeService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMentorRepository, MentorRepository>();
builder.Services.AddScoped<IMentorService, MentorService>();

builder.Services.AddScoped<ILearningTaskRepository, LearningTaskRepository>();
builder.Services.AddScoped<ILearningTaskService, LearningTaskService>();

builder.Services.AddScoped<ITaskAssignmentRepository, TaskAssignmentRepository>();
builder.Services.AddScoped<ITaskAssignmentService, TaskAssignmentService>();

builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();

builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped<IReviewService, ReviewService>();

builder.Logging.AddConsole(); // for loggin
builder.Logging.AddDebug();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () =>
{
    return "Backend is running properly";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler(options =>
{
    options.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/json";
 
        var exceptionFeature = context.Features.Get<IExceptionHandler>();
        if (exceptionFeature is not null)
        {
            var error = new { message = "An unexpected error occurred" };
            await context.Response.WriteAsJsonAsync(error);
        }
    });
});

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.Run();