using System.ComponentModel.DataAnnotations;
using TestingApi.Helpers.ValidationAttributes;
using TestingApi.Models.Test;

namespace TestingApi.Dto.QuestionsPoolTemplateDto;

public class QuestionsPoolTmplDto
{
    public string? DefaultName { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "NumOfQuestionsToBeGenerated must be greater than 0")]
    public int? NumOfQuestionsToBeGeneratedRestriction { get; set; }
    
    [EnumValue(typeof(GenerationStrategy))]
    public string? GenerationStrategyRestriction { get; set; }
}