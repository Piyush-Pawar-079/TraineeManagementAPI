using System.Security.Cryptography;
using AutoMapper;
using CommonLibrary.Contract;
using traineeManagementAPI.DTO.SubmissionFileDTOs;
using traineeManagementAPI.Exceptions;
using CommonLibrary.Models;
using traineeManagementAPI.Repositories.SubmissionFileRepository;
using traineeManagementAPI.Service.PublisherService;

namespace traineeManagementAPI.Service.FileStorageService;

public class LocalFileStorageService: IFileStorageService
{
    private readonly ISubmissionFileRepository _repo;
    private readonly string _uploadPath;
    private readonly ILogger<LocalFileStorageService> _logger;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly RabbitMqSubmissionPublisher _publisher;
    private readonly string[] allowedExtensions = [".jpg", ".png", ".pdf", ".txt", ".zip"];

    public LocalFileStorageService(RabbitMqSubmissionPublisher publisher, ISubmissionFileRepository repo, ILogger<LocalFileStorageService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _repo = repo;
        _logger = logger;
        _uploadPath = "uploads";
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
        _publisher = publisher;

        if (!Directory.Exists(_uploadPath))
        {
            Directory.CreateDirectory(_uploadPath);
        }
    }

    public async Task<SubmissionFileResponseDTO> SaveAsync(int submissionId, CreateSubmissionFileDTO createDTO, CancellationToken cancellationToken)
    {
        if (createDTO.File == null || createDTO.File.Length == 0)
        {
            _logger.LogError("File upload failed");
            throw new ArgumentException("No file selected to upload");
        }
        // if (createDTO.File.Length > 5 * 1024 * 1024)
        // {
        //     _logger.LogError("File upload failed, file is too large");
        //     throw new BadRequestException("File too large. Max 5 MB allowed.");
        // }

        var extention = Path.GetExtension(createDTO.File.FileName).ToLowerInvariant();

        if (string.IsNullOrEmpty(extention) || !allowedExtensions.Contains(extention))
        {
            _logger.LogError("File upload failed because of invalid file extention");
            throw new BadRequestException("Invalid File type.");
        }

        var fileName = $"{Guid.NewGuid()}{createDTO.File.FileName}";
        var filePath = Path.Combine(_uploadPath, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await createDTO.File.CopyToAsync(stream, cancellationToken);

        var submissionFile = new SubmissionFile
        {
            OriginalFileName = createDTO.File.FileName,
            GeneratedFileName = fileName,
            ContentType = createDTO.File.ContentType,
            Size = createDTO.File.Length,
            CheckSum = GenerateFileChecksum(filePath),
            OwnerName = "Piyush",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow,
            SubmissionId = submissionId
        };

        var result = await _repo.SaveAsync(submissionFile);

        if (result == null)
        {
            _logger.LogError("Something went wrong while saving the file to the database");
            throw new Exception("Something went wrong while saving the file to the database");
        }
        _logger.LogInformation("File saved successfully");

        var correlationId = _httpContextAccessor.HttpContext?.TraceIdentifier ?? Guid.NewGuid().ToString();
        var job = new ProcessingJob
        {
            CorrelationId = correlationId,
            SubmissionId = submissionId,
            FileId = submissionFile.Id,
            Status = JobStatus.Queued
        };

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
            _logger.LogInformation(503, "Database updated, but processing queue is currently unavailable. Retry later.");
        }
        else
            _logger.LogInformation("Message added to the processing queue");

        var newResult =  _mapper.Map<SubmissionFileResponseDTO>(result);
        newResult.CorrelationId = correlationId;
        return newResult;
    }

    public async Task<(byte[] bytes, string contentType, string fileName)> OpenReadAsync(int id)
    {

        var file = await _repo.GetSubmissionFileById(id);

        if (file == null)
        {
            _logger.LogError($"File with the specified id - {id} not found");
            throw new NotFoundException($"File with the specified id - {id} not found");
        }
        
        var filePath = Path.Combine(_uploadPath, file.GeneratedFileName);

        if (!File.Exists(filePath))
        {
            _logger.LogError("File is not available at the specified path");
            throw new NotFoundException("File not found");
        }

        byte[] bytes = File.ReadAllBytes(filePath);

        return (bytes, file.ContentType, file.OriginalFileName);
    }

    public string GenerateFileChecksum(string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
            throw new ArgumentException("File path is required.");

        if (!File.Exists(filePath))
            throw new FileNotFoundException("File does not exist.", filePath);

        using var sha256 = SHA256.Create();
        using var stream = File.OpenRead(filePath);
        var hashBytes = sha256.ComputeHash(stream);
        return BitConverter.ToString(hashBytes).ToLower();
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _repo.GetSubmissionFileById(id) == null;
    }
    public async Task DeleteAsync(int id)
    {
        var submissionFile = await _repo.GetSubmissionFileById(id);

        if (submissionFile == null)
        {
            _logger.LogError($"File with the specified id - {id} not found");
            throw new NotFoundException($"File with the specified id - {id} not found");
        }

        var filePath = Path.Combine(_uploadPath, submissionFile.GeneratedFileName);

        if (!File.Exists(filePath))
        {
            _logger.LogError("File not found while deleting.");
            throw new NotFoundException("File not found.");
        }

        File.Delete(filePath);
        await _repo.DeleteAsync(submissionFile);
        _logger.LogInformation("File deleted successfully");
    }
}