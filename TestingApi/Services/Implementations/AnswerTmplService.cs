using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.AnswerTemplateDto;
using TestingApi.Models.TestTemplate;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class AnswerTmplService : IAnswerTmplService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<AnswerTmplService> _logger;

    public AnswerTmplService(DataContext dataContext, ILogger<AnswerTmplService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<AnswerTmplResponseDto?> GetAnswerTmplById(Guid id, CancellationToken cancellationToken = default)
    {
        var answerTemplates = await _dataContext.AnswerTemplates
            .FirstOrDefaultAsync(at => at.Id == id, cancellationToken);

        return _mapper.Map<AnswerTmplResponseDto>(answerTemplates);
    }

    public async Task<bool> AnswerTmplExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.AnswerTemplates.AnyAsync(at => at.Id == id, cancellationToken);
    }

    public async Task<AnswerTmplResponseDto> CreateAnswerTmplAsync(Guid questionTemplateId,
        AnswerTmplDto answerTmplDto, CancellationToken cancellationToken = default)
    {
        var answerTemplateToAdd = _mapper.Map<AnswerTemplate>(answerTmplDto);
        answerTemplateToAdd.QuestionTemplateId = questionTemplateId;
        
        var createdAnswer = await _dataContext.AddAsync(answerTemplateToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AnswerTmplResponseDto>(createdAnswer.Entity);    }

    public async Task UpdateAnswerTmplAsync(Guid id, AnswerTmplDto answerTmplDto,
        CancellationToken cancellationToken = default)
    {
        var answerTemplateFounded = await _dataContext.AnswerTemplates
            .FirstAsync(at => at.Id == id, cancellationToken);
        var updatedAnswerTemplate = _mapper.Map<AnswerTemplate>(answerTmplDto);

        answerTemplateFounded.DefaultText = updatedAnswerTemplate.DefaultText;
        answerTemplateFounded.IsCorrectRestriction = updatedAnswerTemplate.IsCorrectRestriction;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAnswerTmplAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var answerTemplateToDelete = await _dataContext.AnswerTemplates
            .FirstAsync(at => at.Id == id, cancellationToken);

        _dataContext.Remove(answerTemplateToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}