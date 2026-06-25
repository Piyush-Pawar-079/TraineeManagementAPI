using System.Net.Http.Json;
using CommonLibrary.DTO;

namespace SubmissionProcessor.Worker.Clients
{
    public class HttpDirectoryClient(HttpClient httpClient, ILogger<HttpDirectoryClient> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<HttpDirectoryClient> _logger = logger;

        public async Task<TraineeProfileResponseDto?> GetTraineeByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Remove("X-Correlation-ID");
                _httpClient.DefaultRequestHeaders.Add("X-Correlation-ID", Guid.NewGuid().ToString());
                var response = await _httpClient.GetAsync($"/api/trainees/{id}", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Request failed with status {StatusCode}", response.StatusCode);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<TraineeProfileResponseDto>(cancellationToken: cancellationToken);

                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Request timed out");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling TrainingDirectory service");
                return null;
            }
        }

        public async Task<TraineeProfileResponseDto?> GetTraineeAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/trainees", cancellationToken);
                Console.WriteLine("THis is the resposne - " + response);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Request failed with status {StatusCode}", response.StatusCode);
                    return null;
                }

                var result = await response.Content.ReadFromJsonAsync<TraineeProfileResponseDto>(cancellationToken: cancellationToken);

                return result;
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Request timed out");
                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling TrainingDirectory service");
                return null;
            }
        }
    }
}