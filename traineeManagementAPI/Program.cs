using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using DotNetEnv;

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
using traineeManagementAPI.Mappings;
using traineeManagementAPI.Service.FileStorageService;
using traineeManagementAPI.Repositories.SubmissionFileRepository;
using traineeManagementAPI.Service.RedisService;
using traineeManagementAPI.Service.PublisherService;
using CommonLibrary.Data;
using traineeManagementAPI.Repositories.ProcessingJobRepository;
using traineeManagementAPI.Service.ProcessingJobService;
using traineeManagementAPI.Service.CorrelationIdService;
using traineeManagementAPI.Middleware;


var builder = WebApplication.CreateBuilder(args);

Env.Load("../CommonLibrary/.env");

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

var redisConn = Environment.GetEnvironmentVariable("RedisConnectionString");

if (string.IsNullOrWhiteSpace(redisConn))
{
   throw new NotFoundException("Redis connection string is missing");
}

builder.Services.AddStackExchangeRedisCache(
   options =>
   {
      options.Configuration = redisConn;
   }
);

// builder.Services.AddScoped<RedisCacheService>();


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

builder.Services.AddAutoMapper(typeof(MappingProfile));

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

builder.Services.AddScoped<IFileStorageService, LocalFileStorageService>();
builder.Services.AddScoped<ISubmissionFileRepository, SubmissionFileRepository>();

builder.Services.AddScoped<IRedisService, RedisService>();

builder.Services.AddScoped<IRabbitMqPublisher, RabbitMqSubmissionPublisher>();
builder.Services.AddScoped<RabbitMqSubmissionPublisher>();

builder.Services.AddScoped<IProcessingJobRepository, ProcessingJobRepository>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();

builder.Services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationIdAccessor>();

app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapControllers();

app.MapGet("/", () =>
{
    return "Backend is running properly";
});

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin");

app.Run();