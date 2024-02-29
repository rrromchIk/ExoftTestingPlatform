using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.AnswerDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class AnswerService : IAnswerService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestService> _logger;

    public AnswerService(DataContext dataContext, ILogger<TestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AnswerResponseDto?> GetAnswerById(Guid id, CancellationToken cancellationToken = default)
    {
        var answers = await _dataContext.Answers
            .FirstOrDefaultAsync(a => a.Id.Equals(id), cancellationToken);

        return _mapper.Map<AnswerResponseDto>(answers);
    }

    public async Task<bool> AnswerExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Answers.AnyAsync(a => a.Id.Equals(id), cancellationToken);
    }

    public async Task<AnswerResponseDto> CreateAnswerAsync(Guid questionId, AnswerDto answerDto, CancellationToken cancellationToken = default)
    {
        var answerToAdd = _mapper.Map<Answer>(answerDto);
        answerToAdd.QuestionId = questionId;
        
        var createdAnswer = await _dataContext.AddAsync(answerToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AnswerResponseDto>(createdAnswer.Entity);
    }

    public async Task<bool> UpdateAnswerAsync(Guid id, AnswerDto answerDto, CancellationToken cancellationToken = default)
    {
        var answerFounded = await _dataContext.Answers
            .FirstAsync(a => a.Id == id, cancellationToken);
        var updatedAnswer = _mapper.Map<Answer>(answerDto);

        _logger.LogInformation(
            "Test to update: {ttu}. Updated test: {ut}",
            JsonSerializer.Serialize(answerFounded),
            JsonSerializer.Serialize(updatedAnswer)
        );
        
        answerFounded.Text = updatedAnswer.Text;
        answerFounded.IsCorrect = updatedAnswer.IsCorrect;

        return await _dataContext.SaveChangesAsync(cancellationToken) >= 0;
    }

    public async Task<bool> DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var answerToDelete = await _dataContext.Answers
            .FirstAsync(a => a.Id.Equals(id), cancellationToken);

        _dataContext.Remove(answerToDelete);
        return await _dataContext.SaveChangesAsync(cancellationToken) > 0;
    }
}