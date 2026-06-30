using AutoMapper;
using TraineeManagement.Api.DTO.SubmissionDTOs;
using TraineeManagement.Api.Exceptions;
using CommonLibrary.Models;
using TraineeManagement.Api.Repositories.SubmissionRepository;
using TraineeManagement.Api.Service.RedisService;
using TraineeManagement.Api.Service.CorrelationIdService;

namespace TraineeManagement.Api.Service.SubmissionService;

public class SubmissionService(ISubmissionRepository repository, ILogger<SubmissionService> logger, IMapper mapper, IRedisService cache, ICorrelationIdAccessor correlationIdAccessor) : ISubmissionService
{
    private readonly ISubmissionRepository _repo = repository;
    private readonly ILogger<SubmissionService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IRedisService _cache = cache;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();

    public async Task<List<SubmissionDetailDTO>> GetAllAsync()
    {
        var allSubmissions = await _repo.GetAllSubmissionsAsync();
        return _mapper.Map<List<SubmissionDetailDTO>>(allSubmissions);
    }

    public async Task<SubmissionBasicDTO> GetSummary(int id)
    {
        var submission = await _repo.GetSubmissionByIdAsync(id);
        return _mapper.Map<SubmissionBasicDTO>(submission);
    }

    public async Task<SubmissionDetailDTO?> GetByIdAsync(int id)
    {
        var key = $"Submission:{id}";
        var Submission = await _cache.GetAsync<SubmissionDetailDTO>(key);

        if (Submission != null)
        {
            _logger.LogInformation("Submission with the specified Id found in redis cache. Cache Hit case. CorrelationId: {CorrelationId}", correlationId);
            return Submission;
        }

        _logger.LogError("Submission not found in redis cache, fetching from database. Cache Miss case. CorrelationId: {CorrelationId}", correlationId);

        var dbSubmission = await _repo.GetSubmissionByIdAsync(id);

        if (dbSubmission == null)
        {
            _logger.LogError("Submission with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Submission with the id - {id} not found");
        }

        await _cache.SetAsync(key, _mapper.Map<SubmissionDetailDTO>(dbSubmission));

        return _mapper.Map<SubmissionDetailDTO>(dbSubmission);

    }

    public async Task<SubmissionDetailDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto)
    {
        Submission newSubmission = new()
        {
            TaskAssignmentId = createSubmissionDto.TaskAssignmentId,
            SubmissionUrl = createSubmissionDto.SubmissionUrl,
            Notes = createSubmissionDto.Notes,
            SubmittedDate = createSubmissionDto.SubmittedDate,
            Status = createSubmissionDto.Status
        };

        // Submission newSubmission = _mapper.Map<Submission>(createSubmissionDto);

        Submission CreatedSubmission = await _repo.CreateSubmissionAsync(newSubmission);

        if (CreatedSubmission == null)
        {
            _logger.LogError("Something went wrong while creating a new Submission. CorrelationId: {CorrelationId}", correlationId);
            throw new Exception("Something went wrong while creating a new Submission.");
        }

        return _mapper.Map<SubmissionDetailDTO>(CreatedSubmission);

    }

}