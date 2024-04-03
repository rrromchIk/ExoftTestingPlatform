using AutoMapper;
using TestingApi.Dto.AnswerDto;
using TestingApi.Dto.AnswerTemplateDto;
using TestingApi.Dto.QuestionDto;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Dto.QuestionsPoolTemplateDto;
using TestingApi.Dto.QuestionTemplateDto;
using TestingApi.Dto.TestDto;
using TestingApi.Dto.TestTemplateDto;
using TestingApi.Dto.UserAnswerDto;
using TestingApi.Dto.UserDto;
using TestingApi.Dto.UserQuestionDto;
using TestingApi.Dto.UserTestDto;
using TestingApi.Models;
using TestingApi.Models.Test;
using TestingApi.Models.TestTemplate;
using TestingApi.Models.User;

namespace TestingApi.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        ConfigureMappingForTestEntity();
        ConfigureMappingForQuestionPoolEntity();
        ConfigureMappingForQuestionEntity();
        ConfigureMappingForAnswerEntity();
        ConfigureMappingForUserEntity();
        ConfigureMappingForManyToManyEntities();
        ConfigureMappingForTestTemplateEntity();
        ConfigureMappingForQuestionPoolTemplateEntity();
        ConfigureMappingForQuestionTemplateEntity();
        ConfigureMappingForAnswerTemplateEntity();
    }

    private void ConfigureMappingForTestEntity()
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

        CreateMap<TestUpdateDto, Test>()
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
    }

    private void ConfigureMappingForQuestionPoolEntity()
    {
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
                    .MapFrom(
                        src =>
                            Enum.Parse(typeof(GenerationStrategy), src.GenerationStrategy, true)
                    )
            );
        
        CreateMap<QuestionsPoolUpdateDto, QuestionsPool>()
            .ForMember(
                dest => dest.GenerationStrategy,
                opt => opt
                    .MapFrom(
                        src =>
                            Enum.Parse(typeof(GenerationStrategy), src.GenerationStrategy, true)
                    )
            );
    }

    private void ConfigureMappingForQuestionEntity()
    {
        CreateMap<QuestionWithAnswersDto, Question>();
        CreateMap<QuestionUpdateDto, Question>();
        CreateMap<Question, QuestionResponseDto>();
    }

    private void ConfigureMappingForAnswerEntity()
    {
        CreateMap<AnswerDto, Answer>();
        CreateMap<Answer, AnswerResponseDto>();
        CreateMap<AnswerUpdateDto, Answer>();
    }

    private void ConfigureMappingForUserEntity()
    {
        CreateMap<SecurityUserResponseDto, User>()
            .ForMember(
                dest => dest.UserRole,
                opt => opt
                    .MapFrom(src => 
                        Enum.Parse(typeof(UserRole), src.Role, true))
            );
        
        CreateMap<User, UserResponseDto>()
            .ForMember(
                dest => dest.Role,
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
    }

    private void ConfigureMappingForManyToManyEntities()
    {
        CreateMap<UserAnswerDto, UserAnswer>();
        CreateMap<UserAnswer, UserAnswerResponseDto>();

        CreateMap<UserTest, UserTestResponseDto>()
            .ForMember(
                dest => dest.UserTestStatus,
                opt => opt
                    .MapFrom(src => src.UserTestStatus.ToString())
            );

        CreateMap<UserQuestionDto, UserQuestion>();
    }

    private void ConfigureMappingForTestTemplateEntity()
    {
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
    }

    private void ConfigureMappingForQuestionPoolTemplateEntity()
    {
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
                    .MapFrom(
                        src =>
                            Enum.Parse(typeof(GenerationStrategy), src.GenerationStrategyRestriction, true)
                    )
            );
    }

    private void ConfigureMappingForQuestionTemplateEntity()
    {
        CreateMap<QuestionTmplWithAnswerTmplDto, QuestionTemplate>();
        CreateMap<QuestionTmplDto, QuestionTemplate>();
        CreateMap<QuestionTemplate, QuestionTmplResponseDto>();
    }

    private void ConfigureMappingForAnswerTemplateEntity()
    {
        CreateMap<AnswerTmplDto, AnswerTemplate>();
        CreateMap<AnswerTemplate, AnswerTmplResponseDto>();
    }
}