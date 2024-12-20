namespace RRS.Api.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveFileAsync(Guid restaurantId, IFormFile file, string subFolder, CancellationToken cancellationToken = default);
}
