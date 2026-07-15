using CommonLibrary.Constants;

namespace TraineeManagement.Api.Service.CorrelationIdService;

public class CorrelationIdAccessor(IHttpContextAccessor httpContextAccessor) : ICorrelationIdAccessor
{
    public string GetCorrelationId()
    {
        var context = httpContextAccessor.HttpContext;
        if (context != null && context.Items.ContainsKey(AppConstant.CorrelationHeader))
        {
            return context.Items[AppConstant.CorrelationHeader]?.ToString()!;
        }
        return Guid.NewGuid().ToString(); // fallback if no context
    }
}