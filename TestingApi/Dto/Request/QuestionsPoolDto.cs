using TestingApi.Models;

namespace TestingApi.Dto.Request;

public class QuestionsPoolDto
{
    public Guid TestId { get; set; }
    public string Name { get; set; } = null!;
    public int NumOfQuestionsToBeGenerated { get; set; }
    public string GenerationStrategy { get; set; } = null!;
}