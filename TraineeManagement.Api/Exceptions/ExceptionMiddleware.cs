using System.Net;
using System.Text.Json;

namespace TraineeManagement.Api.Exceptions;

public class ExceptionMiddleware(RequestDelegate next,
                           ILogger<ExceptionMiddleware> logger,
                           IWebHostEnvironment env)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger<ExceptionMiddleware> _logger = logger;
    private readonly IWebHostEnvironment _env = env;

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private async Task HandleException(HttpContext context, Exception ex)
    {
        var traceId = context.TraceIdentifier;

        _logger.LogError(ex, "Unhandled Exception Occurred | TraceId: {TraceId}", traceId);

        context.Response.ContentType = "application/json";

        var response = new ApiErrorResponse
        {
            StatusCode = (int)HttpStatusCode.InternalServerError,
            Message = "Something went wrong. Please contact support.",
            TraceId = traceId
        };

        switch (ex)
        {
            case NotFoundException nf:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                response.Message = nf.Message;
                _logger.LogWarning("NotFound: {Message} | TraceId: {TraceId}", nf.Message, traceId);
                break;

            case BadRequestException br:
                response.StatusCode = (int)HttpStatusCode.BadRequest;
                response.Message = br.Message;
                _logger.LogInformation("BadRequest: {Message} | TraceId: {TraceId}", br.Message, traceId);
                break;

            default:
                if (_env.IsDevelopment())
                {
                    response.Message = ex.Message;
                }
                break;
        }

        context.Response.StatusCode = response.StatusCode;

        var json = JsonSerializer.Serialize(response);
        await context.Response.WriteAsync(json);
    }
}