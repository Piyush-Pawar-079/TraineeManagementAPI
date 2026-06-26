namespace traineeManagementAPI.Middleware;
public class CorrelationIdMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";
    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(RequestDelegate next, ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Get or create correlation ID
        if (!context.Request.Headers.TryGetValue(CorrelationHeader, out var correlationId))
        {
            correlationId = Guid.NewGuid().ToString();
            context.Request.Headers[CorrelationHeader] = correlationId;
        }

        // Add to response headers
        context.Response.Headers[CorrelationHeader] = correlationId;

        // Store in HttpContext for later use
        context.Items[CorrelationHeader] = correlationId;

        // Create a logging scope so all logs automatically include the correlation ID
        using (_logger.BeginScope("{CorrelationId}", correlationId!))
        {
            await _next(context);
        }
    }
}
