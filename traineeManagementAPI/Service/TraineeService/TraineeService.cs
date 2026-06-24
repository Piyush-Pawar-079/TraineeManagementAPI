using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Repositories.TraineeRepository;
using traineeManagementAPI.Helpers;
using CommonLibrary.Models;
using traineeManagementAPI.DTO.HelperDTOs;
using traineeManagementAPI.Exceptions;
using traineeManagementAPI.DTO.TaskAssignmentDTOs;
using AutoMapper;
using traineeManagementAPI.Service.RedisService;

namespace traineeManagementAPI.Service.TraineeService;

public class TraineeService(ITraineeRepository repository, ILogger<TraineeService> logger, IMapper mapper, IRedisService cache) : ITraineeService
{

    private class SortFields
    {
        public const String FirstName = "Firstname";
        public const String LastName = "Lastname";
        public const String Email = "Email";
        public const String TechStack = "TechStack";
        public const String Status = "Status";
    }

    private readonly ITraineeRepository _repository = repository;
    private readonly ILogger<TraineeService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly IRedisService _cache = cache;

    public async Task<List<TraineeDetailDTO>> GetAllTrainees()
    {
        var trainee = await _repository.GetAllAsync();

        return _mapper.Map<List<TraineeDetailDTO>>(trainee);
        
    }

    public async Task<TraineeDetailDTO?> GetTraineeById(int id)
    {

        var key = $"Trainee:{id}";
        var trainee = await _cache.GetAsync<TraineeDetailDTO>(key);

        if (trainee != null)
        {
            _logger.LogInformation("Trainee with the specified Id found in redis cache.");
            return trainee;
        }

        _logger.LogError("Trainee not found in redis cache, fetching from database.");

        var  dbTrainee = await _repository.GetByIdAsync(id);

        if (dbTrainee == null)
        {
            _logger.LogError("Trainee with the specified Id is not available.");
            throw new NotFoundException($"Trainee with the id - {id} not found");
        }

        await _cache.SetAsync(key, _mapper.Map<TraineeDetailDTO>(dbTrainee));

        return _mapper.Map<TraineeDetailDTO>(trainee);

    }

    public async Task<TraineeDetailDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO updateDto)
    {
        var key = $"Trainee:{id}";
        await _cache.RemoveAsync(key);

        var existingTrainee = await _repository.GetByIdAsync(id);

        if (existingTrainee == null)
        {
            _logger.LogError("Trainee with the specified Id is not available.");
            throw new NotFoundException($"Trainee with the id - {id} not found");
        }

        if(updateDto.FirstName != null) 
            existingTrainee.FirstName = updateDto.FirstName;
        
        if(updateDto.LastName != null) 
            existingTrainee.LastName = updateDto.LastName;

        if(updateDto.Email != null) 
            existingTrainee.Email = updateDto.Email;
        
        if(updateDto.TechStack != null) 
            existingTrainee.TechStack = updateDto.TechStack;
        
        if(updateDto.Status.HasValue) 
            existingTrainee.Status = updateDto.Status.Value;

        existingTrainee.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repository.UpdateAsync(id, existingTrainee);

        if(desiredTrainee == null)
        {
            _logger.LogError("Something went wrong while updating a new Trainee.");
            throw new Exception("Something went wrong while updating a new Trainee");
        }

        _logger.LogInformation("Trainee Updated Successfully");
        return _mapper.Map<TraineeDetailDTO>(desiredTrainee);

    }

    public async Task<TraineeDetailDTO> CreateTrainee(CreateTraineeRequestDTO trainee)
    {
        Console.WriteLine(trainee.Status);
        Console.WriteLine(trainee.Status.GetType());
        // var newTrainee = new Trainee
        // {
        //     FirstName = trainee.FirstName,
        //     LastName = trainee.LastName,
        //     Email = trainee.Email,
        //     TechStack = trainee.TechStack,
        //     Status = trainee.Status,
        //     CreatedDate = DateTime.UtcNow,
        //     UpdatedDate = DateTime.UtcNow
        // };

        var newTrainee = _mapper.Map<Trainee>(trainee);

        var createdTrainee = await _repository.CreateAsync(newTrainee);

        _logger.LogInformation("Trainee Created Successfully");
        return _mapper.Map<TraineeDetailDTO>(createdTrainee);
    }

    public async Task<bool> DeleteTrainee(int id)
    {
        await _cache.RemoveAsync($"Trainee:{id}");
        _logger.LogInformation("Trainee Deleted Successfully");
        return await _repository.DeleteAsync(id);
    }

    private static List<Trainee> Search(string searchParam, List<Trainee> trainees)
    {
        var desiredTrainee = trainees.Where(
            t => 
            t.FirstName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 || 
            t.LastName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.Email.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.TechStack.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 
        );

        return desiredTrainee.ToList();
    }

    private static List<Trainee> Sort(string sortParam, bool ascending, List<Trainee> trainees)
    {

        // var trainees = await _repository.GetAllAsync();
        
        IEnumerable<Trainee> sorted;

        if (Equals(sortParam, SortFields.FirstName))
            sorted = ascending ? trainees.OrderBy(t => t.FirstName) : trainees.OrderByDescending(t => t.FirstName); 
        else if (Equals(sortParam, SortFields.LastName))
            sorted = ascending ? trainees.OrderBy(t => t.LastName) : trainees.OrderByDescending(t => t.LastName); 
        else if (Equals(sortParam, SortFields.Email))
            sorted = ascending ? trainees.OrderBy(t => t.Email) : trainees.OrderByDescending(t => t.Email); 
        else if (Equals(sortParam, SortFields.TechStack))
            sorted = ascending ? trainees.OrderBy(t => t.TechStack) : trainees.OrderByDescending(t => t.TechStack); 
        else
            sorted = ascending ? trainees.OrderBy(t => t.Status) : trainees.OrderByDescending(t => t.Status); 
    
        return sorted.ToList();
    }

    private async Task<List<Trainee>> GetTraineeUsingPagination(PaginationParams paginationParams, List<Trainee> trainees)
    {
        var paginatedResponse = await _repository.PaginatedResponse(paginationParams, trainees);

        return paginatedResponse.Data.ToList();
    }

    public async Task<List<TraineeDetailDTO>> GetAllAsyncWithFilters(FilterDTO filters, PaginationParams paginationParams)
    {
        var allTrainees = await _repository.GetAllAsync();

        if (filters.SearchParam != null)
        {
            allTrainees = Search(filters.SearchParam, allTrainees);
        }

        if (filters.StatusFilter.HasValue)
        {
            allTrainees = allTrainees.Where(t => String.Equals(t.Status.ToString(), filters.StatusFilter.ToString())).ToList();
        }

        if(filters.SortParam != null)
        {
            allTrainees = Sort(filters.SortParam, filters.Ascending != false, allTrainees);
        }

        if (paginationParams != null)
        {
            allTrainees = await GetTraineeUsingPagination(paginationParams, allTrainees);
        }

        return _mapper.Map<List<TraineeDetailDTO>>(allTrainees);

    }


}