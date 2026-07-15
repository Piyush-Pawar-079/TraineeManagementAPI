using System.Security.Cryptography;
using AutoMapper;
using CommonLibrary.Contract;
using TraineeManagement.Api.DTO.SubmissionFileDTOs;
using TraineeManagement.Api.Exceptions;
using CommonLibrary.Models;
using TraineeManagement.Api.Repositories.SubmissionFileRepository;
using TraineeManagement.Api.Service.PublisherService;
using TraineeManagement.Api.Service.ProcessingJobService;
using TraineeManagement.Api.Service.CorrelationIdService;
using CommonLibrary.Constants;
using System.Security.Claims;

namespace TraineeManagement.Api.Service.FileStorageService;

public class LocalFileStorageService : IFileStorageService
{
    private readonly ISubmissionFileRepository _repo;
    private readonly string _uploadPath;
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly IProcessingJobService _jobService;
    private readonly IMapper _mapper;
    private readonly string correlationId;
    private readonly IRabbitMqPublisher _publisher;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LocalFileStorageService(IRabbitMqPublisher publisher, ISubmissionFileRepository repo, ILogger<LocalFileStorageService> logger, IMapper mapper, IProcessingJobService jobService, ICorrelationIdAccessor correlationIdAccessor, IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _logger = logger;
        _uploadPath = AppConstant.FileUploadPath;
        _mapper = mapper;
        correlationId = correlationIdAccessor.GetCorrelationId();
        _publisher = publisher;
        _jobService = jobService;
        _httpContextAccessor = httpContextAccessor;

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<SubmissionFileResponseDTO> SaveAsync(int submissionId, CreateSubmissionFileDTO createDTO, CancellationToken cancellationToken)
    {
        if (createDTO.File == null || createDTO.File.Length == 0)
        {
            _logger.LogDebug("File upload failed. CorrelationId: {CorrelationId}", correlationId);
            throw new ArgumentException("No file selected to upload");
        }
        if (createDTO.File.Length > AppConstant.AllowedFileSize)
        {
            _logger.LogDebug("File upload failed, file is too large");
            throw new BadRequestException("File too large. Max 5 MB allowed.");
        }

        var extention = Path.GetExtension(createDTO.File.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(extention) || !AppConstant.AllowedFileExtensions.Contains(extention))
        {
            _logger.LogDebug("File upload failed because of invalid file extention. CorrelationId: {CorrelationId}", correlationId);
            throw new BadRequestException("Invalid File type.");
        }

        var fileName = $"{Guid.NewGuid()}{createDTO.File.FileName}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await createDTO.File.CopyToAsync(stream, cancellationToken);

        int userId = int.Parse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value!);

        var submissionFile = new SubmissionFile
        {
            OriginalFileName = createDTO.File.FileName,
            GeneratedFileName = fileName,
            ContentType = createDTO.File.ContentType,
            Size = createDTO.File.Length,
            CheckSum = GenerateFileChecksum(filePath),
            UploadedByUserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            SubmissionId = submissionId
        };

        var result = await _repo.SaveAsync(submissionFile);

        if (result == null)
        {
            _logger.LogDebug("Something went wrong while saving the file to the database. CorrelationId: {CorrelationId}", correlationId);
            throw new Exception("Something went wrong while saving the file to the database");
        }
        _logger.LogInformation("File saved successfully. CorrelationId: {CorrelationId}", correlationId);

        var job = new ProcessingJob
        {
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = submissionFile.Id,
            Status = JobStatus.Queued
        };

        await _jobService.AddProcessingJob(job);

        // 2. Publish messaging work item out of the critical request path
        var message = new SubmissionProcessingRequested
        {
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = submissionFile.Id.ToString(),
            RequestedAt = DateTime.UtcNow
        };

        bool isQueued = await _publisher.PublishSubmissionRequestedAsync(message);

        if (!isQueued)
        {
            _logger.LogInformation(503, "Database updated, message not published to the queue. Retrying message publishing. CorrelationId: {CorrelationId}", correlationId);

            var retry = await _jobService.RetryJobAsync(job.Id);

            if (retry == null)
            {
                _logger.LogInformation(503, "Message not published to the queue. CorrelationId: {CorrelationId}", correlationId);
                throw new Exception("Message not published to the queue, deleting meta data from the database, please retry again later.");
            }

            _logger.LogInformation("Message added to the processing queue. CorrelationId: {CorrelationId}", correlationId);
        }
        else
            _logger.LogInformation("Message added to the processing queue. CorrelationId: {CorrelationId}", correlationId);

        var newResult = _mapper.Map<SubmissionFileResponseDTO>(result);
        newResult.CorrelationId = correlationId;
        return newResult;
    }

    public async Task<(byte[] bytes, string contentType, string fileName)> OpenReadAsync(int id)
    {

        var file = await _repo.GetSubmissionFileById(id);
        // Console.Write("THis is the file: " + file.Id);
        if (file == null)
        {
            _logger.LogDebug($"File with the specified id - {id} not found. CorrelationId: {correlationId}");
            throw new NotFoundException($"File with the specified id - {id} not found");
        }

        var filePath = Path.Combine(_uploadPath, file.GeneratedFileName);

        if (!File.Exists(filePath))
        {
            _logger.LogDebug("File is not available at the specified path. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException("File not found");
        }

        byte[] bytes = File.ReadAllBytes(filePath);
        _logger.LogInformation("File ready to be downloaded. CorrelationId: {CorrelationId}", correlationId);
        return (bytes, file.ContentType, file.OriginalFileName);
    }

    public string GenerateFileChecksum(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            _logger.LogInformation("File path is not present. CorrelationId: {CorrelationId}", correlationId);
            throw new ArgumentException("File path is required.");
        }

        if (!File.Exists(filePath))
        {
            _logger.LogInformation("File does not exist in the path specified. CorrelationId: {CorrelationId}", correlationId);
            throw new FileNotFoundException("File does not exist.", filePath);
        }

        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).ToLower();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repo.GetSubmissionFileById(id) != null;
    }
    public async Task DeleteAsync(int id)
    {
        var submissionFile = await _repo.GetSubmissionFileById(id);

        if (submissionFile == null)
        {
            _logger.LogDebug($"File with the specified id - {id} not found. CorrelationId: {correlationId}");
            throw new NotFoundException($"File with the specified id - {id} not found");
        }

        var filePath = Path.Combine(_uploadPath, submissionFile.GeneratedFileName);

        if (!File.Exists(filePath))
        {
            _logger.LogDebug("File not found while deleting. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException("File not found.");
        }

        File.Delete(filePath);
        await _repo.DeleteAsync(submissionFile);
        _logger.LogInformation("File deleted successfully. CorrelationId: {CorrelationId}", correlationId);
    }
}