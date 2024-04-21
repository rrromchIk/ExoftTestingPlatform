namespace TestingApi.Dto.TestDto;

public class TestFiltersDto : FiltersDto
{
    public string? Difficulty { get; set; }
    public bool? Published { get; set; }
    public string? TemplateId { get; set; }
}