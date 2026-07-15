using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using DotNetEnv;

using TraineeManagement.Api.Exceptions;

using TraineeManagement.Api.Repositories.LearningTaskRepository;
using TraineeManagement.Api.Repositories.MentorRepository;
using TraineeManagement.Api.Repositories.ReviewRepository;
using TraineeManagement.Api.Repositories.SubmissionRepository;
using TraineeManagement.Api.Repositories.TaskAssignmentRepository;
using TraineeManagement.Api.Repositories.TraineeRepository;
using TraineeManagement.Api.Repositories.UserRepository;

using TraineeManagement.Api.Service.AuthService;
using TraineeManagement.Api.Service.LearningTaskService;
using TraineeManagement.Api.Service.MentorService;
using TraineeManagement.Api.Service.ReviewService;
using TraineeManagement.Api.Service.SubmissionService;
using TraineeManagement.Api.Service.TaskAssignmentService;
using TraineeManagement.Api.Service.TraineeService;
using TraineeManagement.Api.Mappings;
using TraineeManagement.Api.Service.FileStorageService;
using TraineeManagement.Api.Repositories.SubmissionFileRepository;
using TraineeManagement.Api.Service.RedisService;
using TraineeManagement.Api.Service.PublisherService;
using CommonLibrary.Data;
using TraineeManagement.Api.Repositories.ProcessingJobRepository;
using TraineeManagement.Api.Service.ProcessingJobService;
using TraineeManagement.Api.Service.CorrelationIdService;
using TraineeManagement.Api.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RabbitMQ.Client;
using CommonLibrary.Configurations;


var builder = WebApplication.CreateBuilder(args);

Env.Load("../CommonLibrary/.env");
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddOpenApi();

// bind custom configurations
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection(JwtConfig.SectionName));
builder.Services.Configure<RedisConfig>(builder.Configuration.GetSection(RedisConfig.SectionName));
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(RabbitMqConfig.SectionName));

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

RedisConfig redisConfig = builder.Configuration.GetSection(RedisConfig.SectionName).Get<RedisConfig>()!;
RabbitMqConfig rabbitMqConfig = builder.Configuration.GetSection(RabbitMqConfig.SectionName).Get<RabbitMqConfig>()!;
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;
Console.WriteLine(connectionString + "tshi is the ocnnection string");
JwtConfig jwtConfig = builder.Configuration.GetSection(JwtConfig.SectionName).Get<JwtConfig>()!;

builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

builder.Services.AddHealthChecks()
   .AddMySql(
      connectionString: connectionString,
      name: "mysql",
      timeout: TimeSpan.FromSeconds(5))
   .AddRedis(
      redisConnectionString: redisConfig.ConnectionString,
      name: "redis",
      timeout: TimeSpan.FromSeconds(5))
   .AddRabbitMQ(
      async sp =>
      {
         var factory = new ConnectionFactory
         {
            HostName = rabbitMqConfig.HostName,
            Port = rabbitMqConfig.Port,
            VirtualHost = rabbitMqConfig.VirtualHost,
            UserName = rabbitMqConfig.UserName,
            Password = rabbitMqConfig.Password
         };
         return await factory.CreateConnectionAsync();
      },
      name: "rabbitmq",
      failureStatus: HealthStatus.Unhealthy,
      timeout: TimeSpan.FromSeconds(5),
      tags: new[] { "mq", "rabbit" }
    )
    ;

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

builder.Services.AddStackExchangeRedisCache(
   options =>
   {
      options.Configuration = redisConfig.ConnectionString;
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
          ValidIssuer = jwtConfig.Issuer,
          ValidAudience = jwtConfig.Audience,
          IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtConfig.Key!)),
          ClockSkew = TimeSpan.Zero,
          RoleClaimType = "Role"
       };
    });

builder.Services.AddOpenApiDocument(config =>
{
   config.DocumentName = "v1";
   config.Title = "Training Management API";

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
// builder.Services.AddScoped<RabbitMqSubmissionPublisher>();

builder.Services.AddScoped<IProcessingJobRepository, ProcessingJobRepository>();
builder.Services.AddScoped<IProcessingJobService, ProcessingJobService>();

builder.Services.AddScoped<ICorrelationIdAccessor, CorrelationIdAccessor>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


app.UseAuthentication();
app.UseAuthorization();

app.UseOpenApi();
app.UseSwaggerUI();

app.UseStaticFiles();

app.MapControllers();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<CorrelationIdMiddleware>();

app.MapHealthChecks("/health");

app.MapHealthChecks("/health/ready", new HealthCheckOptions
{
   Predicate = _ => true, // Include all health checks
   ResponseWriter = async (context, report) =>
   {
      context.Response.ContentType = "application/json";

      var result = new
      {
         status = report.Status.ToString(),
         checks = report.Entries.Select(e => new
         {
            name = e.Key,
            status = e.Value.Status.ToString(),
            description = e.Value.Description
         })
      };

      await context.Response.WriteAsJsonAsync(result);
   }
});

app.MapHealthChecks("/health/live", new HealthCheckOptions
{
   Predicate = _ => false
});


// using (var scope = app.Services.CreateScope())
// {
//    var db = scope.ServiceProvider.GetRequiredService<ApplicationDBContext>();
//    db.Database.Migrate();
// }


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