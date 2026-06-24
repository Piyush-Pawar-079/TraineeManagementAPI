using CommonLibrary.Contract;

namespace traineeManagementAPI.Service.PublisherService;

public interface IRabbitMqPublisher
{
    Task<bool> PublishSubmissionRequestedAsync(SubmissionProcessingRequested message);
}
