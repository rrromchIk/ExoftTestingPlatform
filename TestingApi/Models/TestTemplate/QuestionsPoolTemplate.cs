﻿namespace TestingApi.Models;

public class QuestionsPoolTemplate : BaseEntity
{
    public Guid TestTemplateId { get; set; }
    public TestTemplate TestTemplate { get; set; } = null!;

    public string? NameRestriction { get; set; } = null!;

    public int? NumOfQuestionsToBeGeneratedRestriction { get; set; }

    public GenerationStrategy? GenerationStrategyRestriction { get; set; }

    public ICollection<QuestionTemplate> QuestionsTemplates { get; set; } = null!;
}