using SubmissionProcessor.Worker;
using DotNetEnv;
using CommonLibrary.Data;
using Microsoft.EntityFrameworkCore;
using SubmissionProcessor.Worker.Clients;
using Polly;
using System.Net;
using CommonLibrary.Configurations;

Env.Load("../CommonLibrary/.env");
var builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddHostedService<SubmissionConsumerWorker>();
builder.Services.Configure<RabbitMqConfig>(builder.Configuration.GetSection(RabbitMqConfig.SectionName));
builder.Services.AddHttpClient<HttpDirectoryClient>("TrainingDirectory.Api", client =>
   {
   client.BaseAddress = new Uri(builder.Configuration["TraineeDirectoryApi:Url"]!);
   }).ConfigurePrimaryHttpMessageHandler(() =>
    {
        return new SocketsHttpHandler()
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(15)
        };
    })
    .SetHandlerLifetime(Timeout.InfiniteTimeSpan)
    .AddStandardResilienceHandler(options =>
{
 
    options.Retry.MaxRetryAttempts = 5;
    options.Retry.BackoffType = DelayBackoffType.Exponential;
    options.Retry.UseJitter = true;
 
    options.Retry.ShouldHandle = new PredicateBuilder<HttpResponseMessage>()
        .Handle<HttpRequestException>()
        .HandleResult(response =>
        response.StatusCode == HttpStatusCode.RequestTimeout ||
        response.StatusCode == HttpStatusCode.ServiceUnavailable ||
        response.StatusCode == HttpStatusCode.TooManyRequests ||
        (int)response.StatusCode >= 500
        );
 
    options.CircuitBreaker.FailureRatio = 0.5;
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(20);
    options.CircuitBreaker.MinimumThroughput = 5;
    options.CircuitBreaker.BreakDuration = TimeSpan.FromSeconds(30);
 
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(10);
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var host = builder.Build();

host.Run();
