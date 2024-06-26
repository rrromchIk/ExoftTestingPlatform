﻿using System.Collections;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.UserAnswerDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class UserAnswerService : IUserAnswerService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<UserAnswerService> _logger;

    public UserAnswerService(DataContext dataContext, ILogger<UserAnswerService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ICollection<UserAnswerResponseDto>> GetUserAnswersAsync(Guid userId, Guid questionId,
        CancellationToken cancellationToken = default)
    {
        var userAnswers = await _dataContext.UserAnswers
            .Where(ua => ua.UserId == userId && ua.QuestionId == questionId)
            .ToListAsync(cancellationToken);

        return _mapper.Map<ICollection<UserAnswerResponseDto>>(userAnswers);
    }

    public async Task<bool> UserAnswerExistAsync(Guid userId, Guid questionId, Guid answerId,
        CancellationToken cancellationToken = default)
    {
        return await _dataContext.UserAnswers.AnyAsync(
            ua => ua.UserId == userId &&
                  ua.QuestionId == questionId &&
                  ua.AnswerId == answerId,
            cancellationToken
        );
    }

    public async Task CreateUserAnswersAsync(ICollection<UserAnswerDto> userAnswers,
        CancellationToken cancellationToken = default)
    {
        var userAnswersToCreate = _mapper.Map<ICollection<UserAnswer>>(userAnswers);
        foreach (var userAnswer in userAnswersToCreate)
        {
            userAnswer.AnsweringTime = DateTime.Now;
        }
        
        await _dataContext.AddRangeAsync(userAnswersToCreate, cancellationToken);
        
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteUserAnswerAsync(Guid userId, Guid questionId, Guid answerId,
        CancellationToken cancellationToken = default)
    {
        var userAnswerToDelete = await _dataContext.UserAnswers
            .FirstAsync(
                ua => ua.UserId == userId &&
                      ua.QuestionId == questionId &&
                      ua.AnswerId == answerId,
                cancellationToken
            );
        _dataContext.Remove(userAnswerToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}