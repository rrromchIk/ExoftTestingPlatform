using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.Request;
using TestingApi.Dto.Response;
using TestingApi.Helpers;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionsPoolService : IQuestionsPoolService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestService> _logger;

    public QuestionsPoolService(DataContext dataContext, ILogger<TestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<PagedList<QuestionsPoolResponseDto>> GetQuestionPoolsByTestIdAsync(Guid testId,
        CancellationToken cancellationToken = default)
    {
        var test = await _dataContext.Tests
            .Include(t => t.QuestionsPools)
            .FirstAsync(t => t.Id == testId, cancellationToken);
        var questionsPools = test.QuestionsPools;

        return _mapper.Map<PagedList<QuestionsPoolResponseDto>>(questionsPools);
    }

    public async Task<QuestionsPoolResponseDto> GetQuestionPoolByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var questionsPool = await _dataContext.QuestionsPools
            .FirstAsync(qp => qp.Id.Equals(id), cancellationToken);

        return _mapper.Map<QuestionsPoolResponseDto>(questionsPool);
    }

    public async Task<bool> QuestionsPoolExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPools.AnyAsync(qp => qp.Id.Equals(id), cancellationToken);
    }

    public async Task<QuestionsPoolResponseDto> CreateQuestionsPoolAsync(QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "{dt}. Create questions pool method. TestDto: {dto}",
            DateTime.Now.ToString(),
            JsonSerializer.Serialize(questionsPoolDto)
        );
        var questionsPoolToAdd = _mapper.Map<QuestionsPool>(questionsPoolDto);
        var createdTest = await _dataContext.AddAsync(questionsPoolToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionsPoolResponseDto>(createdTest.Entity);
    }

    public async Task<bool> UpdateQuestionsPoolAsync(Guid id, QuestionsPoolDto questionsPoolDto,
        CancellationToken cancellationToken = default)
    {
        var questionsPoolFounded = await _dataContext.QuestionsPools
            .FirstAsync(qp => qp.Id == id, cancellationToken);
        var updatedQuestionsPool = _mapper.Map<QuestionsPool>(questionsPoolDto);

        _logger.LogInformation(
            "Test to update: {ttu}. Updated test: {ut}",
            JsonSerializer.Serialize(questionsPoolFounded),
            JsonSerializer.Serialize(updatedQuestionsPool)
        );

        questionsPoolFounded.Name = updatedQuestionsPool.Name;
        questionsPoolFounded.NumOfQuestionsToBeGenerated = updatedQuestionsPool.NumOfQuestionsToBeGenerated;
        questionsPoolFounded.GenerationStrategy = updatedQuestionsPool.GenerationStrategy;

        return await _dataContext.SaveChangesAsync(cancellationToken) >= 0;
    }

    public async Task<bool> DeleteQuestionsPoolAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionsPoolToDelete = await _dataContext.QuestionsPools
            .FirstAsync(qp => qp.Id.Equals(id), cancellationToken);

        _dataContext.Remove(questionsPoolToDelete);
        return await _dataContext.SaveChangesAsync(cancellationToken) > 0;
    }
}