namespace FinanceManager.Application.Common.Interfaces;

public interface IFileStorageService
{
    public Task<string> SaveFileAsync(Stream fileStream, string originalFileName,
        CancellationToken cancellationToken = default);

    public Task<Stream> GetFileAsync(string fileReference, CancellationToken cancellationToken = default);

    public Task<bool> FileExistsAsync(string fileReference, CancellationToken cancellationToken = default);

    public Task DeleteFileAsync(string fileReference, CancellationToken cancellationToken = default);
}
