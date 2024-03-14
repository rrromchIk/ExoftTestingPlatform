namespace TestingApi.Dto.TestResultDto;

public class QuestionResultResponseDto
{
    public Guid Id { get; set; }
    public string QuestionText { get; set; } = null!;
    public int MaxScore { get; set; }
    public float UserScore { get; set; }

    public ICollection<AnswerResultResponseDto> AnswersResults { get; set; } = null!;
}