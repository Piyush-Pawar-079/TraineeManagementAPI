using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using DotNetEnv;

using traineeManagementAPI.Data;
using traineeManagementAPI.Exceptions;

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

Env.Load();

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

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
 
builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration.GetValue<string>("Token:Issuer"),
            ValidAudience = builder.Configuration.GetValue<string>("Token:Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("Key")!)),
            ClockSkew = TimeSpan.Zero,
            RoleClaimType = "Role"
        };
    });

builder.Configuration
    .AddEnvironmentVariables();

builder.Services.AddOpenApiDocument(config =>
{
   config.DocumentName = "v1";
   config.Title = "Training Management api";
 
   config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
   {
      Type = OpenApiSecuritySchemeType.ApiKey,
      Scheme = "Bearer",
      In = OpenApiSecurityApiKeyLocation.Header,
      Name = "Authorization",
      Description = "Bearer {your JWT token}"
   });
 
   config.OperationProcessors.Add(
      new AspNetCoreOperationSecurityScopeProcessor("JWT")
   );
});

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

app.UseMiddleware<ExceptionMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi();
app.UseSwaggerUI();

app.MapControllers();

app.MapGet("/", () =>
{
    return "Backend is running properly";
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseExceptionHandler(options =>
// {
//     options.Run(async context =>
//     {
//         context.Response.StatusCode = StatusCodes.Status500InternalServerError;
//         context.Response.ContentType = "application/json";
 
//         var exceptionFeature = context.Features.Get<IExceptionHandler>();
//         if (exceptionFeature is not null)
//         {
//             var error = new { message = "An unexpected error occurred. Please try again later." };
//             await context.Response.WriteAsJsonAsync(error);
//         }
//     });
// });

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.Run();