using AutoMapper;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories.TaskAssignmentRepository;
using traineeManagementAPI.Service.RedisService;

namespace traineeManagementAPI.Service.TaskAssignmentService;

public class TaskAssignmentService(ITaskAssignmentRepository repository, ILogger<TaskAssignmentService> logger, IMapper mapper, IRedisService cache) : ITaskAssignmentService
{
    private readonly ITaskAssignmentRepository _repo = repository;
    private readonly ILogger<TaskAssignmentService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IRedisService _cache = cache;

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
            _logger.LogInformation("taskAssignment with the specified Id found in redis cache.");
            return taskAssignment;
        }

        _logger.LogError("taskAssignment not found in redis cache, fetching from database.");

        var  dbtaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (dbtaskAssignment == null)
        {
            _logger.LogError("Task Assignment with the specified Id is not available.");
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
        var existingTaskAssignment = await _repo.GetTaskAssignmentByIdAsync(id);

        if (existingTaskAssignment == null)
        {
            _logger.LogError("Task Assignment with the specified Id is not available.");
            throw new NotFoundException($"Task Assignment with the id - {id} not found");
        }
        
        if(updateTaskAssignmentDto.Status != null) 
            existingTaskAssignment.Status = updateTaskAssignmentDto.Status.Value;

        var desiredTaskAssignment = await _repo.UpdateTaskAssignmentAsync(id, existingTaskAssignment);

        if(desiredTaskAssignment == null)
        {
            _logger.LogError("Something went wrong while updating a new Task Assignment.");
            throw new Exception("Something went wrong while updating a new Task Assignment");   
        }
        _logger.LogInformation("Task Assignment Updated Successfully");
        return _mapper.Map<TaskAssignmentDetailDTO>(desiredTaskAssignment);
    }
}