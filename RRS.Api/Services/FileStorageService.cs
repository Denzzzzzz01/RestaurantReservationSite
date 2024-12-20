using RRS.Api.Interfaces;

namespace RRS.Api.Services;

public class FileStorageService : IFileStorageService
{
    public async Task<string> SaveFileAsync(Guid restaurantId, IFormFile file, string subFolder, CancellationToken cancellationToken = default)
    {
        if (file == null || file.Length == 0)
            throw new ArgumentException("No file uploaded.");

        var uploadsPath = Path.Combine("uploads", subFolder);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        var fileName = $"{restaurantId}_{Path.GetRandomFileName()}{Path.GetExtension(file.FileName)}";
        var filePath = Path.Combine(uploadsPath, fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream, cancellationToken);
        }

        return $"/uploads/{subFolder}/{fileName}";
    }
}
