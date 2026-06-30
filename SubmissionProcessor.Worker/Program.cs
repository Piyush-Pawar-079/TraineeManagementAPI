using SubmissionProcessor.Worker;
using DotNetEnv;
using CommonLibrary.Data;
using Microsoft.EntityFrameworkCore;
using SubmissionProcessor.Worker.Clients;
using Polly;
using System.Net;

var builder = Host.CreateApplicationBuilder(args);
Env.Load("../CommonLibrary/.env");

builder.Services.AddHostedService<SubmissionConsumerWorker>();

builder.Services.AddHttpClient<HttpDirectoryClient>("TrainingDirectory.Api", client =>
   {
   client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("Training_Directory_API_Base_URL") ?? "http://training_directory_api:8080/");
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


var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);

var host = builder.Build();

host.Run();
