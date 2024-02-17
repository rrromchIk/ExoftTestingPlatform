namespace TestingApi.Models; 

public class UserTest {
    public Guid UserId { get; set; }
    public User User { get; set; } = null!;
    
    public Guid TestId { get; set; }
    public Test Test { get; set; } = null!;
    
    public float Result { get; set; }
    public DateTime StartingTime { get; set; }
    public DateTime EndingTime { get; set; }
    public UserTestStatus UserTestStatus { get; set; }
}