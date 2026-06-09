using Microsoft.VisualBasic;
using traineeManagementAPI.DTO;
using traineeManagementAPI.Model;
using traineeManagementAPI.Repositories;

namespace traineeManagementAPI.Service;

public class TraineeService : ITraineeService
{
    private readonly ITraineeRepository _repository;

    public TraineeService(ITraineeRepository repository)
    {
        _repository = repository;
    }

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

        existingTrainee.UpdatedDate = DateTime.Now;

        var desiredTrainee = await _repository.UpdateAsync(id, existingTrainee);

        if(desiredTrainee == null)
            return null;

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
            CreatedDate = DateTime.Now,
            UpdatedDate = DateTime.Now
        };

        _nextId++;

        var createdTrainee = await _repository.CreateAsync(newTrainee);

        return MapToDTO(createdTrainee);
    }

    public async Task<bool> DeleteTrainee(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    public async Task<List<TraineeResponseDTO>> Search(String searchParam)
    {
        var trainees = await _repository.GetAllAsync();

        var desiredTrainee = trainees.Where(
            t => 
            t.FirstName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 || 
            t.LastName.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.Email.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 ||
            t.TechStack.IndexOf(searchParam, StringComparison.OrdinalIgnoreCase) >= 0 
        );

        return desiredTrainee.Select(MapToDTO).ToList();
    }

    public async Task<List<TraineeResponseDTO>> Sort(string sortParam)
    {

        var trainees = await _repository.GetAllAsync();
        
        if (Equals(sortParam, "firstname"))
        {
            return trainees.OrderBy(t => t.FirstName).Select(MapToDTO).ToList();
        }
        else if (Equals(sortParam, "lastname"))
        {
            return trainees.OrderBy(t => t.LastName).Select(MapToDTO).ToList();
        }
        else if (Equals(sortParam, "email"))
        {
            return trainees.OrderBy(t => t.Email).Select(MapToDTO).ToList();
        }
        else if (Equals(sortParam, "techstack"))
        {
            return trainees.OrderBy(t => t.TechStack).Select(MapToDTO).ToList();
        }
        else
        {
            return trainees.OrderBy(t => t.Status).Select(MapToDTO).ToList();
        }
    }
}