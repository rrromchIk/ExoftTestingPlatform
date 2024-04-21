using TestingApi.Dto.QuestionsPoolDto;

namespace TestingApi.Dto.TestDto;

public class TestWithQuestionsPoolResponseDto : BaseResponseDto
{
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int Duration { get; set; }
    public bool IsPublished { get; set; }
    public Guid? TemplateId { get; set; }
    public string Difficulty { get; set; } = null!;
    public ICollection<QuestionsPoolResponseDto> QuestionsPools { get; set; } = null!;
}