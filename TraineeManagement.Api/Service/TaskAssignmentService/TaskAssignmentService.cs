using AutoMapper;
using TraineeManagement.Api.DTO.TaskAssignmentDTOs;
using TraineeManagement.Api.Exceptions;
using CommonLibrary.Models;
using TraineeManagement.Api.Repositories.TaskAssignmentRepository;
using TraineeManagement.Api.Service.RedisService;
using TraineeManagement.Api.Service.CorrelationIdService;

namespace TraineeManagement.Api.Service.TaskAssignmentService;

public class TaskAssignmentService(ITaskAssignmentRepository repository, ILogger<TaskAssignmentService> logger, IMapper mapper, IRedisService cache, ICorrelationIdAccessor correlationIdAccessor) : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _repo = repository;
    private readonly ILogger<TaskAssignmentService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IRedisService _cache = cache;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();

    public async Task<List<TaskAssignmentDetailDTO>> GetAllAsync()
    {
        var data = await _repo.GetAllTaskAssignmentAsync();
        return _mapper.Map<List<TaskAssignmentDetailDTO>>(data);
    }

    public async Task<TaskAssignmentDetailDTO?> GetByIdAsync(int id)
    {
        var key = $"TaskAssignment:{id}";
        var taskAssignment = await _cache.GetAsync<TaskAssignmentDetailDTO>(key);

        if (taskAssignment != null)
        {
            _logger.LogInformation("taskAssignment with the specified Id found in redis cache. Cache hit case. CorrelationId: {CorrelationId}", correlationId);
            return taskAssignment;
        }

        _logger.LogError("taskAssignment not found in redis cache, fetching from database. Cache miss case. CorrelationId: {CorrelationId}", correlationId);

        var dbtaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (dbtaskAssignment == null)
        {
            _logger.LogError("Task Assignment with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Task Assignment with the id - {id} not found");
        }

        await _cache.SetAsync(key, _mapper.Map<TaskAssignmentDetailDTO>(dbtaskAssignment), 15);

        return _mapper.Map<TaskAssignmentDetailDTO>(dbtaskAssignment);

    }

    public async Task<TaskAssignmentDetailDTO> CreateAsync(CreateTaskAssignmentRequestDTO dto)
    {
        var entity = _mapper.Map<TaskAssignment>(dto);

        var created = await _repo.CreateTaskAssignmentAsync(entity);

        return _mapper.Map<TaskAssignmentDetailDTO>(created);
    }


    public async Task<TaskAssignmentDetailDTO?> UpdateAsync(int id, UpdateTaskAssignmentRequestDTO updateTaskAssignmentDto)
    {
        await _cache.RemoveAsync($"TaskAssignment:{id}");
        _logger.LogInformation("Task Assignment cache invalidation while updating. CorrelationId: {CorrelationId}", correlationId);
        var existingTaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (existingTaskAssignment == null)
        {
            _logger.LogError("Task Assignment with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Task Assignment with the id - {id} not found");
        }

        if (updateTaskAssignmentDto.Status.HasValue)
            existingTaskAssignment.Status = updateTaskAssignmentDto.Status.Value;

        var desiredTaskAssignment = await _repo.UpdateTaskAssignmentAsync(id, existingTaskAssignment);

        if (desiredTaskAssignment == null)
        {
            _logger.LogError("Something went wrong while updating a new Task Assignment. CorrelationId: {CorrelationId}", correlationId);
            throw new Exception("Something went wrong while updating a new Task Assignment");
        }
        _logger.LogInformation("Task Assignment Updated Successfully");
        return _mapper.Map<TaskAssignmentDetailDTO>(desiredTaskAssignment);
    }
}