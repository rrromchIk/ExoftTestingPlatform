using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingAPI.Exceptions;
using TestingApi.Models.TestTemplate;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionsPoolTmplService : IQuestionsPoolTmplService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionsPoolTmplService> _logger;

    public QuestionsPoolTmplService(DataContext dataContext, ILogger<QuestionsPoolTmplService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<QuestionsPoolTmplResponseDto?> GetQuestionPoolTmplByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionsPoolTemplate = await _dataContext.QuestionsPoolTemplates
            .FirstOrDefaultAsync(qp => qp.Id == id, cancellationToken);

        return _mapper.Map<QuestionsPoolTmplResponseDto>(questionsPoolTemplate);
    }

    public async Task<bool> QuestionsPoolTmplExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPoolTemplates
            .AnyAsync(qp => qp.Id == id, cancellationToken);
    }

    public async Task<QuestionsPoolTmplResponseDto> CreateQuestionsPoolTmplAsync(Guid testTemplateId,
        QuestionsPoolTmplDto questionsPoolTmplDto, CancellationToken cancellationToken = default)
    {
        var questionsPoolTemplateToAdd = _mapper.Map<QuestionsPoolTemplate>(questionsPoolTmplDto);
        questionsPoolTemplateToAdd.TestTemplateId = testTemplateId;

        var collision = await _dataContext.QuestionsPoolTemplates
            .Where(qp => qp.DefaultName != null)
            .AnyAsync(
                qp =>
                    qp.DefaultName == questionsPoolTemplateToAdd.DefaultName &&
                    qp.TestTemplateId == questionsPoolTemplateToAdd.TestTemplateId,
                cancellationToken
            );

        if (collision)
            throw new ApiException(
                "Questions pool template name has to be unique for the each test template",
                StatusCodes.Status409Conflict
            );

        var createdQuestionsPoolTemplate = await _dataContext.AddAsync(questionsPoolTemplateToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionsPoolTmplResponseDto>(createdQuestionsPoolTemplate.Entity);
    }

    public async Task UpdateQuestionsPoolTmplAsync(Guid id, QuestionsPoolTmplDto questionsPoolTmplDto,
        CancellationToken cancellationToken = default)
    {
        var questionsPoolTemplateFounded = await _dataContext.QuestionsPoolTemplates
            .FirstAsync(qp => qp.Id == id, cancellationToken);
        var updatedQuestionsPoolTemplate = _mapper.Map<QuestionsPoolTemplate>(questionsPoolTmplDto);
        
        var collision = await _dataContext.QuestionsPoolTemplates
            .Where(qp => qp.DefaultName != null)
            .AnyAsync(
                qp =>
                    qp.DefaultName == updatedQuestionsPoolTemplate.DefaultName &&
                    qp.TestTemplateId == updatedQuestionsPoolTemplate.TestTemplateId &&
                    qp.Id != questionsPoolTemplateFounded.Id,
                cancellationToken
            );

        if (collision)
            throw new ApiException(
                "Questions pool template name has to be unique for the each test",
                StatusCodes.Status409Conflict
            );

        questionsPoolTemplateFounded.DefaultName = updatedQuestionsPoolTemplate.DefaultName;
        questionsPoolTemplateFounded.NumOfQuestionsToBeGeneratedRestriction = 
            updatedQuestionsPoolTemplate.NumOfQuestionsToBeGeneratedRestriction;
        questionsPoolTemplateFounded.GenerationStrategyRestriction = updatedQuestionsPoolTemplate.GenerationStrategyRestriction;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteQuestionsPoolTmplAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionsPoolTemplateToDelete = await _dataContext.QuestionsPoolTemplates
            .FirstAsync(qp => qp.Id == id, cancellationToken);

        _dataContext.Remove(questionsPoolTemplateToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}