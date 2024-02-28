﻿using System.Data;
using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.QuestionsPoolDto;
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
    
    public async Task<QuestionsPoolResponseDto?> GetQuestionPoolByIdAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var questionsPool = await _dataContext.QuestionsPools
            .FirstOrDefaultAsync(qp => qp.Id.Equals(id), cancellationToken);

        return _mapper.Map<QuestionsPoolResponseDto>(questionsPool);
    }

    public async Task<bool> QuestionsPoolExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.QuestionsPools.AnyAsync(qp => qp.Id.Equals(id), cancellationToken);
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
        
        var collision = await _dataContext.QuestionsPools.AnyAsync(
            qp => qp.Name == updatedQuestionsPool.Name 
                  && qp.TestId == updatedQuestionsPool.TestId
                  && qp.Id != questionsPoolFounded.Id,
            cancellationToken
        );
        
        if (collision)
            throw new DataException("Questions pool name has to be unique for the each test");

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