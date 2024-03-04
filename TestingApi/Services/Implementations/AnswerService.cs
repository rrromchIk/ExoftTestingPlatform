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
    private readonly ILogger<AnswerService> _logger;

    public AnswerService(DataContext dataContext, ILogger<AnswerService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<AnswerResponseDto?> GetAnswerById(Guid id, CancellationToken cancellationToken = default)
    {
        var answers = await _dataContext.Answers
            .FirstOrDefaultAsync(a => a.Id == id, cancellationToken);

        return _mapper.Map<AnswerResponseDto>(answers);
    }

    public async Task<bool> AnswerExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Answers.AnyAsync(a => a.Id == id, cancellationToken);
    }

    public async Task<AnswerResponseDto> CreateAnswerAsync(Guid questionId, AnswerDto answerDto, CancellationToken cancellationToken = default)
    {
        var answerToAdd = _mapper.Map<Answer>(answerDto);
        answerToAdd.QuestionId = questionId;
        
        var createdAnswer = await _dataContext.AddAsync(answerToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<AnswerResponseDto>(createdAnswer.Entity);
    }

    public async Task UpdateAnswerAsync(Guid id, AnswerDto answerDto, CancellationToken cancellationToken = default)
    {
        var answerFounded = await _dataContext.Answers
            .FirstAsync(a => a.Id == id, cancellationToken);
        var updatedAnswer = _mapper.Map<Answer>(answerDto);
        
        answerFounded.Text = updatedAnswer.Text;
        answerFounded.IsCorrect = updatedAnswer.IsCorrect;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAnswerAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var answerToDelete = await _dataContext.Answers
            .FirstAsync(a => a.Id == id, cancellationToken);

        _dataContext.Remove(answerToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }
}