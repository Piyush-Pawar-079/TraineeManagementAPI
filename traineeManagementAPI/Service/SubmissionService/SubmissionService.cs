using AutoMapper;
using traineeManagementAPI.DTO.SubmissionDTOs;
using traineeManagementAPI.Exceptions;
using CommonLibrary.Models;
using traineeManagementAPI.Repositories.SubmissionRepository;
using traineeManagementAPI.Service.RedisService;

namespace traineeManagementAPI.Service.SubmissionService;

public class SubmissionService(ISubmissionRepository repository, ILogger<SubmissionService> logger, IMapper mapper, IRedisService cache) : ISubmissionService
{
    private readonly ISubmissionRepository _repo = repository;
    private readonly ILogger<SubmissionService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IRedisService _cache = cache;

    public async Task<List<SubmissionDetailDTO>> GetAllAsync()
    {
        var allSubmissions = await _repo.GetAllSubmissionsAsync();
        return _mapper.Map<List<SubmissionDetailDTO>>(allSubmissions);
    }

    public async Task<SubmissionDetailDTO?> GetByIdAsync(int id)
    {
        // var desiredSubmission = await _repo.GetSubmissionByIdAsync(id);

        // if (desiredSubmission == null)
        // {
        //     _logger.LogError("Submission with the specified Id is not available.");
        //     throw new NotFoundException($"Submission with the id - {id} not found");
        // }

        // return _mapper.Map<SubmissionDetailDTO>(desiredSubmission);

        var key = $"Submission:{id}";
        var Submission = await _cache.GetAsync<SubmissionDetailDTO>(key);

        if (Submission != null)
        {
            _logger.LogInformation("Submission with the specified Id found in redis cache.");
            return Submission;
        }

        _logger.LogError("Submission not found in redis cache, fetching from database.");

        var  dbSubmission = await _repo.GetSubmissionByIdAsync(id);

        if (dbSubmission == null)
        {
            _logger.LogError("Submission with the specified Id is not available.");
            throw new NotFoundException($"Submission with the id - {id} not found");
        }

        await _cache.SetAsync(key, _mapper.Map<SubmissionDetailDTO>(dbSubmission));

        return _mapper.Map<SubmissionDetailDTO>(Submission);

    }

    public async Task<SubmissionDetailDTO> CreateAsync(CreateSubmissionRequestDTO createSubmissionDto)
    {
        // Submission newSubmission = new()
        // {
        //     TaskAssignmentId = createSubmissionDto.TaskAssignmentId,
        //     SubmissionUrl = createSubmissionDto.SubmissionUrl,
        //     Notes = createSubmissionDto.Notes,
        //     SubmittedDate = createSubmissionDto.SubmittedDate,
        //     Status = createSubmissionDto.Status
        // };

        Submission newSubmission = _mapper.Map<Submission>(createSubmissionDto);

        Submission CreatedSubmission = await _repo.CreateSubmissionAsync(newSubmission);

        if (CreatedSubmission == null)
        {
            _logger.LogError("Something went wrong while creating a new Submission.");
            throw new Exception("Something went wrong while creating a new Submission");
        }

        return _mapper.Map<SubmissionDetailDTO>(CreatedSubmission);

    }

}