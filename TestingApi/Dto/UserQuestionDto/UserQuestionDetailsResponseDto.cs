namespace TestingApi.Dto.UserQuestionDto;

public class UserQuestionDetailsResponseDto
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public bool IsAnswered { get; set; }
}