using AutoMapper;
using TestingApi.Dto.AnswerDto;
using TestingApi.Dto.QuestionDto;
using TestingApi.Dto.QuestionsPoolDto;
using TestingApi.Dto.TestDto;
using TestingApi.Models;

namespace TestingApi.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<TestDto, Test>()
            .ForMember(
                dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => src.Difficulty.ToString())
            );

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

        CreateMap<PagedList<Test>, PagedList<TestResponseDto>>()
            .ConvertUsing(
                (src, dest, context) =>
                {
                    var mappedItems = context.Mapper.Map<List<TestResponseDto>>(src.Items);
                    return new PagedList<TestResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                }
            );


        CreateMap<QuestionsPool, QuestionsPoolDto>()
            .ForMember(
                dest => dest.GenerationStrategy,
                opt => opt
                    .MapFrom(src => src.GenerationStrategy.ToString())
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
        
        CreateMap<PagedList<QuestionsPool>, PagedList<QuestionsPoolResponseDto>>()
            .ConvertUsing(
                (src, dest, context) =>
                {
                    var mappedItems = context.Mapper.Map<List<QuestionsPoolResponseDto>>(src.Items);
                    return new PagedList<QuestionsPoolResponseDto>(mappedItems, src.Page, src.PageSize, src.TotalCount);
                }
            );

        CreateMap<QuestionWithAnswersDto, Question>();
        CreateMap<QuestionDto, Question>();
        CreateMap<Question, QuestionResponseDto>();
        
        CreateMap<AnswerDto, Answer>();
        CreateMap<Answer, AnswerResponseDto>();
    }
}