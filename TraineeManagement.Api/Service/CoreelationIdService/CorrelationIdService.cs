namespace TraineeManagement.Api.Service.CorrelationIdService;

public class CorrelationIdAccessor : ICorrelationIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private const string CorrelationHeader = "X-Correlation-ID";

    public CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetCorrelationId()
    {
        var context = _httpContextAccessor.HttpContext;
        if (context != null && context.Items.ContainsKey(CorrelationHeader))
        {
            return context.Items[CorrelationHeader]?.ToString()!;
        }
        return Guid.NewGuid().ToString(); // fallback if no context
    }
}