﻿namespace TestingApi.Dto.TestDto;

public class TestResponseDto : BaseResponseDto
{
    public string Name { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public int Duration { get; set; }
    public bool IsPublished { get; set; }
    public string Difficulty { get; set; } = null!;
    public Guid? TemplateId { get; set; }
}