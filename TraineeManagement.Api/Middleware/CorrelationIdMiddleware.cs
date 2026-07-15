using CommonLibrary.Constants;

namespace TraineeManagement.Api.Middleware;

public class CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
{
    

    public async Task InvokeAsync(HttpContext context)
    {
        // Get or create correlation ID
        if (!context.Request.Headers.TryGetValue(AppConstant.CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[AppConstant.CorrelationHeader] = correlationId;
        }

        // Add to response headers
        context.Response.Headers[AppConstant.CorrelationHeader] = correlationId;

        // Store in HttpContext for later use
        context.Items[AppConstant.CorrelationHeader] = correlationId;

        // Create a logging scope so all logs automatically include the correlation ID
        using (logger.BeginScope("{CorrelationId}", correlationId!))
        {
            await next(context);
        }
    }
}
