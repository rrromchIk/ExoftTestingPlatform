namespace TestingApi.Dto.StatisticDto;

public class UserStatisticResponseDto
{
    public int AmountOfStartedTests { get; set; }
    public int AmountOfCompletedTests { get; set; }
    public int AmountOfInProcessTests { get; set; }
    public float? AverageResult { get; set; }
    public ICollection<float> AllTestsResults { get; set; } = null!;
}