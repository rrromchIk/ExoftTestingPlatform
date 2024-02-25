using TestingApi.Models;

namespace TestingApi.Dto.Response;

public class QuestionsPoolResponseDto : BaseResponseDto
{
    public Guid TestId { get; set; }
    public string Name { get; set; } = null!;
    public int NumOfQuestionsToBeGenerated { get; set; }
    public string GenerationStrategy { get; set; } = null!;
}