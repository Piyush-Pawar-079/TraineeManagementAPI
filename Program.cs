using Microsoft.EntityFrameworkCore;
using traineeManagementAPI.Data;
using traineeManagementAPI.Repositories.TraineeRepository;
using traineeManagementAPI.Repositories.UserRepository;
using traineeManagementAPI.Service.AuthService;
using traineeManagementAPI.Service.TraineeService;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();
builder.Services.AddScoped<ITraineeService, TraineeService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
 
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

builder.Services.AddScoped<ITraineeRepository, TraineeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();


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

app.Run();