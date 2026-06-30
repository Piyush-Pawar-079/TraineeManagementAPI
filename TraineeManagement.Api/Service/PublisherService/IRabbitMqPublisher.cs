using CommonLibrary.Contract;

namespace TraineeManagement.Api.Service.PublisherService;

public interface IRabbitMqPublisher
{
    Task<bool> PublishSubmissionRequestedAsync(SubmissionProcessingRequested message);
}
