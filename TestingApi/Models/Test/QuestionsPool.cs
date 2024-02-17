namespace TestingApi.Models; 

public class QuestionsPool : BaseEntity {
    public Guid TestId { get; set; }
    public Test Test { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    public int NumOfQuestionsToBeGenerated { get; set; }
    public GenerationStrategy GenerationStrategy { get; set; }

    public ICollection<Question> Questions { get; set; } = null!;
    
    
}