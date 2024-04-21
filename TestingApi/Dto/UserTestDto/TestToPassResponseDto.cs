namespace TestingApi.Dto.UserTestDto;

public class TestToPassResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int Duration { get; set; }
    public string Difficulty { get; set; } = null!;
    public DateTime CreatedTimestamp { get; set; }

    public string UserTestStatus { get; set; } = null!;
}