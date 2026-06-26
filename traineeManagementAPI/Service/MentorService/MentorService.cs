using AutoMapper;
using Azure.Core;
using traineeManagementAPI.DTO.MentorDTOs;
using traineeManagementAPI.Exceptions;
using CommonLibrary.Models;
using traineeManagementAPI.Repositories.MentorRepository;
using traineeManagementAPI.Service.CorrelationIdService;
using traineeManagementAPI.DTO.AuthDTOs;

namespace traineeManagementAPI.Service.MentorService;

public class MentorService(IMentorRepository repository, ILogger<MentorService> logger, IMapper mapper, ICorrelationIdAccessor correlationIdAccessor) : IMentorService
{
    private readonly IMentorRepository _repo = repository;
    private readonly ILogger<MentorService> _logger = logger;
    private readonly IMapper _mapper = mapper;
    private readonly string correlationId = correlationIdAccessor.GetCorrelationId();

    public async Task<List<MentorDetailDTO>> GetAllAsync()
    {
        var allMentors = await _repo.GetAllAsync();
        _logger.LogInformation("Getting all the mentors. CorrelationId: {CorrelationId}", correlationId);
        return _mapper.Map<List<MentorDetailDTO>>(allMentors);
    }

    public async Task<MentorDetailDTO?> GetByIdAsync(int id)
    {
        var desiredMentor = await _repo.GetByIdAsync(id);

        if (desiredMentor == null)
        {
            _logger.LogError("Mentor with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Mentor with the id - {id} not found");
        }

        return _mapper.Map<MentorDetailDTO>(desiredMentor);

    }

    public async Task<MentorDetailDTO> CreateAsync(CreateMentorRequestDTO createMentorDto)
    {
        // Mentor newMentor = new()
        // {
        //     FirstName = createMentorDto.FirstName,
        //     LastName = createMentorDto.LastName,
        //     Email = createMentorDto.Email,
        //     Expertise = createMentorDto.Expertise,
        //     Status = createMentorDto.Status,
        //     CreatedDate = DateTime.UtcNow,
        //     UpdatedDate = DateTime.UtcNow
        // };

        Mentor newMentor = _mapper.Map<Mentor>(createMentorDto);

        Mentor CreatedMentor = await _repo.CreateAsync(newMentor);

        _logger.LogInformation("Mentor Created Successfully. CorrelationId: {CorrelationId}", correlationId);

        return _mapper.Map<MentorDetailDTO>(CreatedMentor);

    }

    public async Task<MentorDetailDTO?> UpdateAsync(int id, UpdateMentorRequestDTO updateMentorDto)
    {
        var existingMentor = await _repo.GetByIdAsync(id);

        if (existingMentor == null)
        {
            _logger.LogError("Mentor with the specified Id is not available. CorrelationId: {CorrelationId}", correlationId);
            throw new NotFoundException($"Mentor with the id - {id} not found");
        }

        if(updateMentorDto.FirstName != null) 
            existingMentor.FirstName = updateMentorDto.FirstName;
        
        if(updateMentorDto.LastName != null) 
            existingMentor.LastName = updateMentorDto.LastName;

        if(updateMentorDto.Email != null) 
            existingMentor.Email = updateMentorDto.Email;
        
        if(updateMentorDto.Expertise != null) 
            existingMentor.Expertise = updateMentorDto.Expertise;
        
        if(updateMentorDto.Status.HasValue) 
            existingMentor.Status = updateMentorDto.Status.Value;

        existingMentor.UpdatedDate = DateTime.UtcNow;

        var desiredMentor = await _repo.UpdateAsync(id, existingMentor);

        if(desiredMentor == null)
        {
            _logger.LogError("Something went wrong while updating a new Mentor. CorrelationId: {CorrelationId}", correlationId);
            throw new Exception("Something went wrong while updating a new Mentor");
        }

        _logger.LogInformation("Mentor Updated Successfully. CorrelationId: {CorrelationId}", correlationId);
        return _mapper.Map<MentorDetailDTO>(desiredMentor);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        _logger.LogInformation("Mentor deleted successfully. CorrelationId: {CorrelationId}", correlationId);
        return await _repo.DeleteAsync(id);
    }

    public async Task SaveChangesAsync()
    {
        await _repo.SaveChangesAsync();
    }

}