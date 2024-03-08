using System.ComponentModel.DataAnnotations;

namespace TestingApi.Dto.QuestionTemplateDto;

public class QuestionTmplWithAnswerTmplDto
{
    public string? DefaultText { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "MaxScore must be greater than 0")]
    public int? MaxScore { get; set; }

    public ICollection<AnswerTemplateDto.AnswerTmplDto> AnswerTemplates { get; set; } = null!;
}