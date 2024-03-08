using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TestingApi.Data;
using TestingApi.Dto;
using TestingApi.Dto.TestTemplateDto;
using TestingAPI.Exceptions;
using TestingApi.Helpers;
using TestingApi.Models.TestTemplate;
using TestingApi.Services.Abstractions;

namespace TestingApi.Services.Implementations;

public class TestTemplateService : ITestTemplateService
{
    private readonly DataContext _dataContext;
    private readonly IMapper _mapper;
    private readonly ILogger<TestTemplateService> _logger;

    public TestTemplateService(DataContext dataContext, ILogger<TestTemplateService> logger, IMapper mapper)
    {
        _dataContext = dataContext;
        _mapper = mapper;
        _logger = logger;
    }
    
    public async Task<TestTemplateResponseDto?> GetTestTemplateByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var testTemplate = await _dataContext.TestTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return _mapper.Map<TestTemplateResponseDto>(testTemplate);
    }

    public async Task<TestTemplateWithQpTemplatesResponseDto?> GetTestTemplateWithQuestionsPoolsTemplatesByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var testTemplate = await _dataContext.TestTemplates
            .AsNoTracking()
            .Include(t => t.QuestionsPoolTemplates)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);

        return _mapper.Map<TestTemplateWithQpTemplatesResponseDto>(testTemplate);
    }

    public async Task<bool> TestTemplateExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _dataContext.TestTemplates
            .AnyAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<TestTemplateWithQpTemplatesResponseDto> CreateTestTemplateAsync(TestTemplateWithQpTemplateDto testWithQuestionsPoolsDto,
        CancellationToken cancellationToken = default) 
    {
        var testTemplateToAdd = _mapper.Map<TestTemplate>(testWithQuestionsPoolsDto);

        if (!testWithQuestionsPoolsDto.QuestionsPoolTemplates.IsNullOrEmpty())
        {
            var amountOfUniqueQuestionsPoolNames = testWithQuestionsPoolsDto.QuestionsPoolTemplates
                .Where(qp => !qp.DefaultName.IsNullOrEmpty())
                .Select(qp => qp.DefaultName)
                .ToHashSet()
                .Count;
            
            if(amountOfUniqueQuestionsPoolNames != testTemplateToAdd.QuestionsPoolTemplates.Count) 
                throw new ApiException("Questions pool template names have to be unique", StatusCodes.Status409Conflict);
            
            testTemplateToAdd.QuestionsPoolTemplates =
                _mapper.Map<ICollection<QuestionsPoolTemplate>>(testWithQuestionsPoolsDto.QuestionsPoolTemplates);
        }

        var collision = await _dataContext.TestTemplates
            .AnyAsync(
                t => t.TemplateName == testTemplateToAdd.TemplateName,
                cancellationToken
            );

        if (collision)
            throw new ApiException("Test template name has to be unique", StatusCodes.Status409Conflict);

        var createdTestTemplate = await _dataContext.AddAsync(testTemplateToAdd, cancellationToken);

        await _dataContext.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TestTemplateWithQpTemplatesResponseDto>(createdTestTemplate.Entity);
    }

    public async Task UpdateTestTemplateAsync(Guid id, TestTemplateDto testTemplateDto, CancellationToken cancellationToken = default)
    {
        var testTemplateFounded = await _dataContext.TestTemplates.FirstAsync(e => e.Id == id, cancellationToken);
        var updatedTestTemplate = _mapper.Map<TestTemplate>(testTemplateDto);
        
        var collision = await _dataContext.TestTemplates
            .AnyAsync(
                t => 
                    t.TemplateName == updatedTestTemplate.TemplateName &&
                    t.Id != testTemplateFounded.Id,
                cancellationToken
            );

        if (collision)
            throw new ApiException("Test template name has to be unique", StatusCodes.Status409Conflict);

        testTemplateFounded.TemplateName = updatedTestTemplate.TemplateName;
        testTemplateFounded.DefaultDuration = updatedTestTemplate.DefaultDuration;
        testTemplateFounded.DefaultSubject = updatedTestTemplate.DefaultSubject;
        testTemplateFounded.DefaultTestDifficulty = updatedTestTemplate.DefaultTestDifficulty;

        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteTestTemplateAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var testTemplateToDelete = await _dataContext.TestTemplates
            .FirstAsync(e => e.Id == id, cancellationToken);

        _dataContext.Remove(testTemplateToDelete);
        await _dataContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<PagedList<TestTemplateResponseDto>> GetAllTestsTemplatesAsync(FiltersDto filtersDto, CancellationToken cancellationToken = default)
    {
        IQueryable<TestTemplate> testTemplatesQuery = _dataContext.TestTemplates;

        if (!string.IsNullOrWhiteSpace(filtersDto.SearchTerm))
        {
            testTemplatesQuery = testTemplatesQuery.Where(
                t =>
                    t.TemplateName.Contains(filtersDto.SearchTerm)
            );
        }

        testTemplatesQuery = filtersDto.SortOrder?.ToLower() == "desc"
            ? testTemplatesQuery.OrderByDescending(GetSortProperty(filtersDto.SortColumn))
            : testTemplatesQuery.OrderBy(GetSortProperty(filtersDto.SortColumn));

        var tests = await PagedList<TestTemplate>.CreateAsync(
            testTemplatesQuery,
            filtersDto.Page,
            filtersDto.PageSize,
            cancellationToken
        );

        return _mapper.Map<PagedList<TestTemplateResponseDto>>(tests);    
    }
    
    private static Expression<Func<TestTemplate, object>> GetSortProperty(string? sortColumn)
    {
        return sortColumn?.ToLower() switch
        {
            "name" => t => t.TemplateName,
            "creationTime" => t => t.CreatedTimestamp,
            _ => t => t.Id
        };
    }
}