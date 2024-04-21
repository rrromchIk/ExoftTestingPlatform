using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Models.Test;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionsPoolService : IQuestionsPoolService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionsPoolService> _logger;

    public QuestionsPoolService(DataContext dataContext, ILogger<QuestionsPoolService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<QuestionsPoolResponseDto?> GetQuestionPoolByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var questionsPool = await _dataContext.QuestionsPools
            .FirstOrDefaultAsync(qp => qp.Id == id, cancellationToken);

        return _mapper.Map<QuestionsPoolResponseDto>(questionsPool);
    }

    public async Task<bool> QuestionsPoolExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPools
            .AnyAsync(qp => qp.Id == id, cancellationToken);
    }

    public async Task<QuestionsPoolResponseDto> CreateQuestionsPoolAsync(Guid testId, QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default)
    {
        var questionsPoolToAdd = _mapper.Map<QuestionsPool>(questionsPoolDto);
        questionsPoolToAdd.TestId = testId;
        
        var createdQuestionsPool = await _dataContext.AddAsync(questionsPoolToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionsPoolResponseDto>(createdQuestionsPool.Entity);
    }

    public async Task UpdateQuestionsPoolAsync(Guid id, QuestionsPoolUpdateDto questionsPoolUpdateDto,
        CancellationToken cancellationToken = default)
    {
        var questionsPoolFounded = await _dataContext.QuestionsPools
            .FirstAsync(qp => qp.Id == id, cancellationToken);
        var updatedQuestionsPool = _mapper.Map<QuestionsPool>(questionsPoolUpdateDto);
        
        questionsPoolFounded.Name = updatedQuestionsPool.Name;
        questionsPoolFounded.NumOfQuestionsToBeGenerated = updatedQuestionsPool.NumOfQuestionsToBeGenerated;
        questionsPoolFounded.GenerationStrategy = updatedQuestionsPool.GenerationStrategy;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteQuestionsPoolAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionsPoolToDelete = await _dataContext.QuestionsPools
            .FirstAsync(qp => qp.Id == id, cancellationToken);

        _dataContext.Remove(questionsPoolToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}