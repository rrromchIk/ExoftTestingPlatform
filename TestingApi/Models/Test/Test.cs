﻿namespace TestingApi.Models; 

public class Test : BaseEntity{
    //has to be unique
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int Duration { get; set; }
    public bool IsPublished { get; set; }
    public TestDifficulty Difficulty { get; set; }
    
    public ICollection<QuestionsPool> QuestionsPools { get; set; } = null!;
    public ICollection<UserTest> UserTests { get; set; } = null!;
}