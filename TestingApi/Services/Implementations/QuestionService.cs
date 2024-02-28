using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingApi.Data;
using TestingApi.Dto.QuestionDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionService : IQuestionService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestService> _logger;

    public QuestionService(DataContext dataContext, ILogger<TestService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<QuestionResponseDto> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questions = await _dataContext.Questions
            .FirstOrDefaultAsync(q => q.Id.Equals(id), cancellationToken);

        return _mapper.Map<QuestionResponseDto>(questions);
    }
    
    public async Task<bool> QuestionExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Questions.AnyAsync(q => q.Id.Equals(id), cancellationToken);
    }
    
    public async Task<QuestionResponseDto> CreateQuestionAsync(QuestionDto questionDto, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "{dt}. Create question method. QuestionDto: {dto}",
            DateTime.Now.ToString(),
            JsonSerializer.Serialize(questionDto)
        );
        var questionToAdd = _mapper.Map<Question>(questionDto);
        
        var createdQuestion = await _dataContext.AddAsync(questionToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionResponseDto>(createdQuestion.Entity);
    }
    
    public async Task<bool> UpdateQuestionAsync(Guid id, QuestionDto questionDto, CancellationToken cancellationToken = default)
    {
        var questionFounded = await _dataContext.Questions
            .FirstAsync(q => q.Id == id, cancellationToken);
        var updatedQuestion = _mapper.Map<Question>(questionDto);

        _logger.LogInformation(
            "Question to update: {ttu}. Updated question: {ut}",
            JsonSerializer.Serialize(questionFounded),
            JsonSerializer.Serialize(updatedQuestion)
        );
        
        questionFounded.Text = updatedQuestion.Text;
        questionFounded.MaxScore = updatedQuestion.MaxScore;

        return await _dataContext.SaveChangesAsync(cancellationToken) >= 0;
    }
    
    public async Task<bool> DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionToDelete = await _dataContext.Questions
            .FirstAsync(q => q.Id.Equals(id), cancellationToken);

        _dataContext.Remove(questionToDelete);
        return await _dataContext.SaveChangesAsync(cancellationToken) > 0;
    }

    public async Task<ICollection<QuestionResponseDto>> GetQuestionsByQuestionsPoolIdAsync(Guid questionsPoolId, CancellationToken cancellationToken = default)
    {
        var questionsPool = await _dataContext.QuestionsPools
            .Include(qp => qp.Questions)
            .FirstAsync(qp => qp.Id == questionsPoolId, cancellationToken);
        var questions = questionsPool.Questions;

        return _mapper.Map<ICollection<QuestionResponseDto>>(questions);
    }
}