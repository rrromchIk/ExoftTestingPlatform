namespace TestingApi.Models;

public class UserAnswer
{
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;

    public Guid AnswerId { get; set; }
    public Answer Answer { get; set; } = null!;
    public DateTime AnsweringTime { get; set; }
}