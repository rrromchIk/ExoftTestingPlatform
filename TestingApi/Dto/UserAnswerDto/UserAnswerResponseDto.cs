namespace TestingApi.Dto.UserAnswerDto;

public class UserAnswerResponseDto
{
    public Guid UserId { get; set; }
    public Guid QuestionId { get; set; }
    public Guid AnswerId { get; set; }
    public DateTime AnsweringTime { get; set; }
}