using TestingApi.Dto.TestDto;

namespace TestingApi.Dto.StatisticDto;

public class TestStatisticResponseDto
{
    public TestResponseDto Test { get; set; } = null!;
    public int TotalAmountOfAttemptsTaken { get; set; }
    public int AmountOfCurrentGoingAttempts { get; set; }
    public float AverageUsersTimeSpent { get; set; }
    public float AverageUsersScore { get; set; }
    public ICollection<float> AllUsersScores { get; set; } = null!;
}