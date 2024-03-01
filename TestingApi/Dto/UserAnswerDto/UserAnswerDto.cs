namespace TestingApi.Dto.UserAnswerDto;

public class UserAnswerDto
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
}