using FinanceManager.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace FinanceManager.Infrastructure.Services.FileStorage;

public class FileStorageService : IFileStorageService
{
    private readonly string _baseDirectory;

    public FileStorageService(IOptions<FileStorageSettings> settings)
    {
        _baseDirectory = settings.Value.BaseDirectory;
        if (!Directory.Exists(_baseDirectory))
        {
            Directory.CreateDirectory(_baseDirectory);
        }
    }

    public async Task<string> SaveFileAsync(Stream fileStream, string originalFileName,
        CancellationToken cancellationToken = default)
    {
        var uniqueFileName = Guid.NewGuid().ToString();
        var filePath = Path.Combine(_baseDirectory, uniqueFileName);

        await using var output = new FileStream(filePath, FileMode.CreateNew, FileAccess.Write, FileShare.None, 4096, useAsync: true);
        await fileStream.CopyToAsync(output, cancellationToken);

        return uniqueFileName;
    }

    public Task<Stream> GetFileAsync(string fileReference, CancellationToken cancellationToken = default)
    {
        var filePath = Path.Combine(_baseDirectory, fileReference);
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File not found: {fileReference}");
        }

        var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 4096, useAsync: true);
        return Task.FromResult<Stream>(fileStream);
    }

    public Task<bool> FileExistsAsync(string fileReference, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var filePath = Path.Combine(_baseDirectory, fileReference);
            return File.Exists(filePath);
        }, cancellationToken);
    }

    public Task DeleteFileAsync(string fileReference, CancellationToken cancellationToken = default)
    {
        return Task.Run(() =>
        {
            var filePath = Path.Combine(_baseDirectory, fileReference);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }, cancellationToken);
    }
}
