﻿using TestingApi.Models.Test;

namespace TestingApi.Models.TestTemplate;

public class AnswerTemplate : BaseEntity
{
    public Guid QuestionTemplateId { get; set; }
    public QuestionTemplate QuestionTemplate { get; set; } = null!;
    public string? DefaultText { get; set; }
    public bool IsCorrectRestriction { get; set; }
    public ICollection<Answer> AnswersFromTemplate { get; set; } = null!;
}
