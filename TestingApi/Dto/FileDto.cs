namespace TestingApi.Dto;

public class FileDto
{
    public byte[] Content { get; set; } = null!;
    public string FileName { get; set; } = null!;
    public string MimeType { get; set; } = null!;
}