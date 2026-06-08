using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
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

    public async Task<List<TraineeResponseDTO>> GetAllTrainees()
    {
        var trainee = await _repository.GetAllAsync();

        return trainee.Select(trainee => new TraineeResponseDTO
        {
            Id = trainee.Id,
            FirstName = trainee.FirstName,
            LastName = trainee.LastName,
            Email = trainee.Email,
            TechStack = trainee.TechStack,
            Status = trainee.Status,
            CreateDate = trainee.CreatedDate,
            UpdateDate = trainee.UpdatedDate
        }).ToList();
        
    }

    public async Task<TraineeResponseDTO?> GetTraineeById(int id)
    {
        var trainee = await _repository.GetByIdAsync(id);

        if (trainee == null)
        {
            return null;
        }

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

        return new TraineeResponseDTO
        {
            Id = desiredTrainee.Id,
            FirstName = desiredTrainee.FirstName,
            LastName = desiredTrainee.LastName,
            Email = desiredTrainee.Email,
            TechStack = desiredTrainee.TechStack,
            Status = desiredTrainee.Status,
            CreateDate = desiredTrainee.CreatedDate,
            UpdateDate = desiredTrainee.UpdatedDate
        };

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


        var createdTrainee = await _repository.CreateAsync(newTrainee);

        return new TraineeResponseDTO
        {
            Id = createdTrainee.Id,
            FirstName = createdTrainee.FirstName,
            LastName = createdTrainee.LastName,
            Email = createdTrainee.Email,
            TechStack = createdTrainee.TechStack,
            Status = createdTrainee.Status,
            CreateDate = createdTrainee.CreatedDate,
            UpdateDate = createdTrainee.UpdatedDate
        };

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
            t.FirstName.Contains(searchParam) ||
            t.LastName.Contains(searchParam) ||
            t.Email.Contains(searchParam) ||
            t.TechStack.Contains(searchParam)
        ).ToList();

        return desiredTrainee.Select(t => new TraineeResponseDTO
        {
            Id = t.Id,
            FirstName = t.FirstName,
            LastName = t.LastName,
            Email = t.Email,
            TechStack = t.TechStack,
            Status = t.Status,
            CreateDate = t.CreatedDate,
            UpdateDate = t.UpdatedDate
        }).ToList();

    }

}