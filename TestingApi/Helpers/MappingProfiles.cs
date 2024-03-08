using AutoMapper;
using TestingApi.Dto.AnswerDto;
using TestingApi.Dto.QuestionDto;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.TestTemplateDto;
using TestingApi.Dto.UserAnswerDto;
using TestingApi.Dto.UserDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Models;
using TestingApi.Models.TestTemplate;

namespace TestingApi.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<Test, TestResponseDto>()
            .ForMember(
                dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => src.Difficulty.ToString())
            );
        
        CreateMap<Test, TestWithQuestionsPoolResponseDto>()
            .ForMember(
                dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => src.Difficulty.ToString())
            );

        CreateMap<TestWithQuestionsPoolsDto, Test>()
            .ForMember(
                dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(TestDifficulty), src.Difficulty, true))
            );
        
        CreateMap<TestDto, Test>()
            .ForMember(
                dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(TestDifficulty), src.Difficulty, true))
            );

        CreateMap<PagedList<Test>, PagedList<TestResponseDto>>()
            .ConvertUsing(
                (src, dest, context) =>
                {
                    var mappedItems = context.Mapper.Map<List<TestResponseDto>>(src.Items);
                    return new PagedList<TestResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                }
            );
        
        CreateMap<QuestionsPool, QuestionsPoolResponseDto>()
            .ForMember(
                dest => dest.GenerationStrategy,
                opt => opt
                    .MapFrom(src => src.GenerationStrategy.ToString())
            );

        CreateMap<QuestionsPoolDto, QuestionsPool>()
            .ForMember(
                dest => dest.GenerationStrategy,
                opt => opt
                    .MapFrom(src => 
                        Enum.Parse(typeof(GenerationStrategy), src.GenerationStrategy, true)
                )
            );
        
        CreateMap<QuestionWithAnswersDto, Question>();
        CreateMap<QuestionDto, Question>();
        CreateMap<Question, QuestionResponseDto>();
        
        CreateMap<AnswerDto, Answer>();
        CreateMap<Answer, AnswerResponseDto>();

        
        CreateMap<UserDto, User>()
            .ForMember(
                dest => dest.UserRole,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(UserRole), src.UserRole, true))
            );

        CreateMap<User, UserResponseDto>()
            .ForMember(
                dest => dest.UserRole,
                opt => opt
                    .MapFrom(src => src.UserRole.ToString())
            );
        
        CreateMap<PagedList<User>, PagedList<UserResponseDto>>()
            .ConvertUsing(
                (src, dest, context) =>
                {
                    var mappedItems = context.Mapper.Map<List<UserResponseDto>>(src.Items);
                    return new PagedList<UserResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                }
            );

        CreateMap<UserAnswerDto, UserAnswer>();
        CreateMap<UserAnswer, UserAnswerResponseDto>();
        
        CreateMap<UserTest, UserTestResponseDto>()
            .ForMember(
                dest => dest.UserTestStatus,
                opt => opt
                    .MapFrom(src => src.UserTestStatus.ToString())
            );
        
        
        CreateMap<TestTemplate, TestTmplResponseDto>()
            .ForMember(
                dest => dest.DefaultTestDifficulty,
                opt => opt
                    .MapFrom(src => src.DefaultTestDifficulty.ToString())
            );
        
        CreateMap<TestTemplate, TestTmplWithQpTmplsResponseDto>()
            .ForMember(
                dest => dest.DefaultTestDifficulty,
                opt => opt
                    .MapFrom(src => src.DefaultTestDifficulty.ToString())
            );

        CreateMap<TestTmplWithQuestionsPoolTmplDto, TestTemplate>()
            .ForMember(
                dest => dest.DefaultTestDifficulty,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(TestDifficulty), src.DefaultTestDifficulty, true))
            );
        
        CreateMap<TestTmplDto, TestTemplate>()
            .ForMember(
                dest => dest.DefaultTestDifficulty,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(TestDifficulty), src.DefaultTestDifficulty, true))
            );

        CreateMap<PagedList<TestTemplate>, PagedList<TestTmplResponseDto>>()
            .ConvertUsing(
                (src, dest, context) =>
                {
                    var mappedItems = context.Mapper.Map<List<TestTmplResponseDto>>(src.Items);
                    return new PagedList<TestTmplResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                }
            );
        
        CreateMap<QuestionsPoolTemplate, QuestionsPoolTmplResponseDto>()
            .ForMember(
                dest => dest.GenerationStrategyRestriction,
                opt => opt
                    .MapFrom(src => src.GenerationStrategyRestriction.ToString())
            );

        CreateMap<QuestionsPoolTmplDto, QuestionsPoolTemplate>()
            .ForMember(
                dest => dest.GenerationStrategyRestriction,
                opt => opt
                    .MapFrom(src => 
                        Enum.Parse(typeof(GenerationStrategy), src.GenerationStrategyRestriction, true)
                    )
            );
    }
}