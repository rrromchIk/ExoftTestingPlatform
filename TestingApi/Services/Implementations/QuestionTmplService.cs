using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Data;
using TestingApi.Dto.QuestionTemplateDto;
using TestingApi.Models.TestTemplate;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionTmplService : IQuestionTmplService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionTmplService> _logger;
    
    public QuestionTmplService(DataContext dataContext, ILogger<QuestionTmplService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<QuestionTmplResponseDto?> GetQuestionTmplByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionTemplates = await _dataContext.QuestionTemplates
            .Include(qt => qt.AnswerTemplates)
            .FirstOrDefaultAsync(qt => qt.Id == id, cancellationToken);

        return _mapper.Map<QuestionTmplResponseDto>(questionTemplates);
    }

    public async Task<bool> QuestionTmplExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionTemplates
            .AnyAsync(qt => qt.Id == id, cancellationToken);
    }

    public async Task<QuestionTmplResponseDto> CreateQuestionTmplAsync(Guid questionsPoolTemplateId,
        QuestionTmplWithAnswerTmplDto questionTmplWithAnswerTmplDto, CancellationToken cancellationToken = default)
    {
        var questionTemplateToAdd = _mapper.Map<QuestionTemplate>(questionTmplWithAnswerTmplDto);
        questionTemplateToAdd.QuestionsPoolTemplateId = questionsPoolTemplateId;
        
        if (!questionTmplWithAnswerTmplDto.AnswerTemplates.IsNullOrEmpty())
        {
            questionTemplateToAdd.AnswerTemplates = _mapper
                .Map<ICollection<AnswerTemplate>>(questionTmplWithAnswerTmplDto.AnswerTemplates);
        }
        
        var createdQuestion = await _dataContext.AddAsync(questionTemplateToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionTmplResponseDto>(createdQuestion.Entity);
    }

    public async Task UpdateQuestionTmplAsync(Guid id, QuestionTmplDto questionTmplDto,
        CancellationToken cancellationToken = default)
    {
        var questionTemplateFounded = await _dataContext.QuestionTemplates
            .FirstAsync(qt => qt.Id == id, cancellationToken);
        
        var updatedQuestionTemplate = _mapper.Map<QuestionTemplate>(questionTmplDto);
        
        questionTemplateFounded.MaxScoreRestriction = updatedQuestionTemplate.MaxScoreRestriction;
        questionTemplateFounded.DefaultText = updatedQuestionTemplate.DefaultText;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteQuestionTmplAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionTemplateToDelete = await _dataContext.QuestionTemplates
            .FirstAsync(qt => qt.Id == id, cancellationToken);

        _dataContext.Remove(questionTemplateToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);    
    }

    public async Task<ICollection<QuestionTmplResponseDto>> GetQuestionTmplsByQuestionsPoolTmplIdAsync(
        Guid questionsPoolTmplId, CancellationToken cancellationToken = default)
    {
        var questionsPoolTemplates = await _dataContext.QuestionsPoolTemplates
            .Include(qp => qp.QuestionsTemplates)
            .ThenInclude(q => q.AnswerTemplates)
            .FirstAsync(qpt => qpt.Id == questionsPoolTmplId, cancellationToken);
        var questionTemplates = questionsPoolTemplates.QuestionsTemplates;

        return _mapper.Map<ICollection<QuestionTmplResponseDto>>(questionTemplates);
    }
}