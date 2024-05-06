using Microsoft.AspNetCore.StaticFiles;
using TestingApi.Dto;
using TestingAPI.Exceptions;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class FileService : IFileService
{
    private readonly IConfiguration _configuration;

    public FileService(IConfiguration configuration)//do not inject configuration. inject model instead
    {
        _configuration = configuration;
    }
    public async Task RemoveFilesByNameIfExistsAsync(string fileName, CancellationToken cancellationToken = default)//there is no need in this method
                                                                                                                    //old file is replaced by new one when created
                                                                                                                    //read FileMode.Create description
    {
        var folderName = _configuration["FileStorage:FolderPath"];
        var pathToRemoveFiles = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        var filesToDelete = Directory.GetFiles(pathToRemoveFiles, fileName + ".*");
        var deleteTasks = filesToDelete
            .Select(
                fileToDelete => Task.Run(
                    () => File.Delete(
                        fileToDelete
                    ),
                    cancellationToken
                )
            );

        await Task.WhenAll(deleteTasks);
    }

    public async Task<string> StoreFileAsync(IFormFile file, string fileName,
        CancellationToken cancellationToken = default)
    {
        var folderName = _configuration["FileStorage:FolderPath"];
        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
        var fileExtension = Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(pathToSave, fileName + fileExtension);
        
        await using var stream = new FileStream(fullPath, FileMode.Create);
        await file.CopyToAsync(stream, cancellationToken);

        return fullPath;
    }

    public async Task<FileDto> GetFileAsync(string filePath, CancellationToken cancellationToken = default)
    {
        if (!File.Exists(filePath))
            throw new ApiException("File doesn't exists", StatusCodes.Status404NotFound);

        var contentTypeProvider = new FileExtensionContentTypeProvider();
        contentTypeProvider.TryGetContentType(filePath, out string mimeType);
        
        return new FileDto
        {
            Content = await File.ReadAllBytesAsync(filePath, cancellationToken),
            FileName = "photo",
            MimeType = mimeType
        };
    }
}