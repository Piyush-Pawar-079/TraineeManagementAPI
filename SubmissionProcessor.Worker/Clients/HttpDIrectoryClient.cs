using System.Net.Http.Json;
using System.Text.Json;
using CommonLibrary.DTO;

namespace SubmissionProcessor.Worker.Clients
{
    public class HttpDirectoryClient(HttpClient httpClient, ILogger<HttpDirectoryClient> logger)
    {
        private readonly HttpClient _httpClient = httpClient;
        private readonly ILogger<HttpDirectoryClient> _logger = logger;

        private readonly JsonSerializerOptions _options = new()
        {
            PropertyNameCaseInsensitive = true
        };

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

                // ✅ DEBUG (optional)
                var raw = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {raw}", raw);

                var result = JsonSerializer.Deserialize<TraineeProfileResponseDto>(raw, _options);

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

        public async Task<List<TraineeProfileResponseDto>> GetTraineeAsync(CancellationToken cancellationToken)
        {
            try
            {
                var response = await _httpClient.GetAsync("/api/trainees", cancellationToken);

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogWarning("Request failed with status {StatusCode}", response.StatusCode);
                    return new List<TraineeProfileResponseDto>();
                }

                var raw = await response.Content.ReadAsStringAsync();
                _logger.LogInformation("API Response: {raw}", raw);

                var result = JsonSerializer.Deserialize<List<TraineeProfileResponseDto>>(raw, _options);

                return result ?? new List<TraineeProfileResponseDto>();
            }
            catch (TaskCanceledException)
            {
                _logger.LogError("Request timed out");
                return new List<TraineeProfileResponseDto>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error calling TrainingDirectory service");
                return new List<TraineeProfileResponseDto>();
            }
        }
    }
}