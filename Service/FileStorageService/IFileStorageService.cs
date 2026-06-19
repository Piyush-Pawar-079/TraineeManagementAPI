namespace traineeManagementAPI.Service.FileStorageService;

public interface IFileStorageService
{
    Task SaveAsync();

    Task OpenReadAsync();

    Task ExistsAsync();

    Task<bool> DeleteAsync();
}