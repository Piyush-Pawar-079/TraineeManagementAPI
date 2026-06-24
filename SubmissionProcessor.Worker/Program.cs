using SubmissionProcessor.Worker;
using DotNetEnv;
using CommonLibrary.Data;
using Microsoft.EntityFrameworkCore;

var builder = Host.CreateApplicationBuilder(args);
Env.Load("../CommonLibrary/.env");

builder.Services.AddHostedService<SubmissionProcessorWorker>();

var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");

builder.Services.AddDbContext<ApplicationDBContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString))
);



var host = builder.Build();
host.Run();
