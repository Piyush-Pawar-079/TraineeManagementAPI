using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddControllers();

builder.Services.AddHealthChecks();

var app = builder.Build();

app.MapControllers();

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
         checks = report.Entries.Select(e => new {
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

app.MapGet("/", () =>
{
    return "Backend is running properly";
});

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.Run();
