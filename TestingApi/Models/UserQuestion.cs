using TestingApi.Models.Test;

namespace TestingApi.Models;

public class UserQuestion
{
    public Guid UserId { get; set; }
    public User.User User { get; set; } = null!;
    
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = null!;
}