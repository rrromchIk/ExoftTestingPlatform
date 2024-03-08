using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionsPoolTemplateDto;

public class QuestionsPoolTemplateDto
{
    public string? DefaultName { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "NumOfQuestionsToBeGenerated must be greater than 0")]
    public int? NumOfQuestionsToBeGeneratedRestriction { get; set; }
    
    [RegularExpression(
        @"^(?i)(sequentially|randomly)$",
        ErrorMessage = "GenerationStrategy must be: sequentially or randomly (case-insensitive)"
    )]
    public string? GenerationStrategyRestriction { get; set; }
}