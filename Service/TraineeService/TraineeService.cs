using traineeManagementAPI.DTO.TraineeDTOs;
using traineeManagementAPI.Repositories.TraineeRepository;
using traineeManagementAPI.Helpers;
using traineeManagementAPI.Model;
using traineeManagementAPI.DTO.HelperDTOs;

namespace traineeManagementAPI.Service.TraineeService;

public class TraineeService(ITraineeRepository repository, ILogger<TraineeService> logger) : ITraineeService
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
    private static int _nextId = 0;

    private TraineeResponseDTO MapToDTO(Trainee trainee)
    {
        return new TraineeResponseDTO
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
            CreateDate = trainee.CreatedDate,
            UpdateDate = trainee.UpdatedDate
        };
    }

    public async Task<List<TraineeResponseDTO>> GetAllTrainees()
    {
        var trainee = await _repository.GetAllAsync();

        return trainee.Select(MapToDTO).ToList();
        
    }

    public async Task<TraineeResponseDTO?> GetTraineeById(int id)
    {
        var trainee = await _repository.GetByIdAsync(id);

        if (trainee == null)
        {
            return null;
        }

        return MapToDTO(trainee);

    }

    public async Task<TraineeResponseDTO?> UpdateTrainee(int id, UpdateTraineeRequestDTO updateDto)
    {
        var existingTrainee = await _repository.GetByIdAsync(id);

        if (existingTrainee == null)
        {
            return null;
        }

        if(updateDto.FirstName != null) 
            existingTrainee.FirstName = updateDto.FirstName;
        
        if(updateDto.LastName != null) 
            existingTrainee.LastName = updateDto.LastName;

        if(updateDto.Email != null) 
            existingTrainee.Email = updateDto.Email;
        
        if(updateDto.TechStack != null) 
            existingTrainee.TechStack = updateDto.TechStack;
        
        if(updateDto.Status != null) 
            existingTrainee.Status = updateDto.Status;

        existingTrainee.UpdatedDate = DateTime.UtcNow;

        var desiredTrainee = await _repository.UpdateAsync(id, existingTrainee);

        if(desiredTrainee == null)
            return null;

        _logger.LogInformation("Trainee Updated Successfully");
        return MapToDTO(desiredTrainee);

    }

    public async Task<TraineeResponseDTO> CreateTrainee(CreateTraineeRequestDTO trainee)
    {
        int traineeId = _nextId++; 
        var newTrainee = new Trainee
        {
            Id = traineeId,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
            CreatedDate = DateTime.UtcNow,
            UpdatedDate = DateTime.UtcNow
        };

        _nextId++;

        var createdTrainee = await _repository.CreateAsync(newTrainee);

        _logger.LogInformation("Trainee Created Successfully");
        return MapToDTO(createdTrainee);
    }

    public async Task<bool> DeleteTrainee(int id)
    {
        _logger.LogInformation("Trainee Deleted Successfully");
        return await _repository.DeleteAsync(id);
    }

    private static async Task<List<Trainee>> Search(String searchParam, List<Trainee> trainees)
    {
        // var trainees = await _repository.GetAllAsync();

        var desiredTrainee = trainees.Where(
            t => 
            t.FirstName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 || 
            t.LastName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.Email.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.TechStack.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 
        );

        return desiredTrainee.ToList();
    }

    private static async Task<List<Trainee>> Sort(string sortParam, bool ascending, List<Trainee> trainees)
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

    public async Task<List<TraineeResponseDTO>> GetAllAsyncWithFilters(FilterDTO filters, PaginationParams paginationParams)
    {
        var allTrainees = await _repository.GetAllAsync();

        if (filters.SearchParam != null)
        {
            allTrainees = await Search(filters.SearchParam, allTrainees);
        }

        if (filters.StatusFilter != null)
        {
            allTrainees = allTrainees.Where(t => String.Equals(t.Status.ToString(), filters.StatusFilter.ToString())).ToList();
        }

        if(filters.SortParam != null)
        {
            allTrainees = await Sort(filters.SortParam, filters.Ascending != false, allTrainees);
        }

        if (paginationParams != null)
        {
            allTrainees = await GetTraineeUsingPagination(paginationParams, allTrainees);
        }

        return allTrainees.Select(MapToDTO).ToList();

    }


}