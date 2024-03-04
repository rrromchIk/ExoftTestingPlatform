using System.Text.Json;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Data;
using TestingApi.Dto.QuestionDto;
using TestingApi.Models;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class QuestionService : IQuestionService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<QuestionService> _logger;

    public QuestionService(DataContext dataContext, ILogger<QuestionService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<QuestionResponseDto?> GetQuestionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questions = await _dataContext.Questions
            .Include(q => q.Answers)
            .FirstOrDefaultAsync(q => q.Id == id, cancellationToken);

        return _mapper.Map<QuestionResponseDto>(questions);
    }
    
    public async Task<bool> QuestionExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.Questions
            .AnyAsync(q => q.Id == id, cancellationToken);
    }
    
    public async Task<QuestionResponseDto> CreateQuestionAsync(Guid questionsPoolId,
        QuestionWithAnswersDto questionWithAnswersDto,
        CancellationToken cancellationToken = default)
    {
        var questionToAdd = _mapper.Map<Question>(questionWithAnswersDto);
        questionToAdd.QuestionsPoolId = questionsPoolId;
        
        if (!questionWithAnswersDto.Answers.IsNullOrEmpty())
        {
            questionToAdd.Answers = _mapper.Map<ICollection<Answer>>(questionWithAnswersDto.Answers);
        }
        
        var createdQuestion = await _dataContext.AddAsync(questionToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<QuestionResponseDto>(createdQuestion.Entity);
    }
    
    public async Task UpdateQuestionAsync(Guid id, QuestionDto questionDto, CancellationToken cancellationToken = default)
    {
        var questionFounded = await _dataContext.Questions
            .FirstAsync(q => q.Id == id, cancellationToken);
        
        var updatedQuestion = _mapper.Map<Question>(questionDto);
        
        questionFounded.Text = updatedQuestion.Text;
        questionFounded.MaxScore = updatedQuestion.MaxScore;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }
    
    public async Task DeleteQuestionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var questionToDelete = await _dataContext.Questions
            .FirstAsync(q => q.Id == id, cancellationToken);

        _dataContext.Remove(questionToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<ICollection<QuestionResponseDto>> GetQuestionsByQuestionsPoolIdAsync(Guid questionsPoolId, CancellationToken cancellationToken = default)
    {
        var questionsPool = await _dataContext.QuestionsPools
            .Include(qp => qp.Questions)
            .ThenInclude(q => q.Answers)
            .FirstAsync(qp => qp.Id == questionsPoolId, cancellationToken);
        var questions = questionsPool.Questions;

        return _mapper.Map<ICollection<QuestionResponseDto>>(questions);
    }
}