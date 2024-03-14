namespace TestingApi.Dto.TestResultDto;

public class AnswerResultResponseDto
{
    public Guid Id { get; set; }
    public string AnswerText { get; set; } = null!;
    public bool IsCorrect { get; set; }
    public bool IsUserAnswerSelected { get; set; }
}