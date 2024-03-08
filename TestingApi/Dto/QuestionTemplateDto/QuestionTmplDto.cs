using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionTemplateDto;

public class QuestionTmplDto
{
    public string? DefaultText { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "MaxScore must be greater than 0")]
    public int? MaxScoreRestriction { get; set; }
}