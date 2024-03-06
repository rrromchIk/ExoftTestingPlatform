using TestingApi.Dto;

namespace TestingApi.Services.Abstractions;

public interface IFileService
{
    Task RemoveFilesIfExistsAsync(string name, CancellationToken cancellationToken = default);
    Task<string> StoreFileAsync(IFormFile file, string fileName, CancellationToken cancellationToken = default);
    Task<FileDto> GetFileAsync(string filePath, CancellationToken cancellationToken = default);
}