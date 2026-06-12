using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;

using traineeManagementAPI.Repositories.LearningTaskRepository;
using traineeManagementAPI.Repositories.MentorRepository;
using traineeManagementAPI.Repositories.TraineeRepository;
using traineeManagementAPI.Repositories.UserRepository;

using traineeManagementAPI.Service.AuthService;
using traineeManagementAPI.Service.LearningTaskService;
using traineeManagementAPI.Service.MentorService;
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

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.Run();