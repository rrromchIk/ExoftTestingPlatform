using AutoMapper;
using TestingApi.Dto;
using TestingApi.Models;

namespace TestingApi.Helpers;

public class MappingProfiles : Profile {
    public MappingProfiles() {
        CreateMap<Test, TestDto>()
            .ForMember(dest => dest.Difficulty, 
                opt => opt
                    .MapFrom(src => src.Difficulty.ToString()));
            
        CreateMap<Test, TestDto>()
            .ForMember(dest => dest.Difficulty, 
                opt => opt
                    .MapFrom(src => src.Difficulty.ToString()));

        CreateMap<TestDto, Test>()
            .ForMember(dest => dest.Difficulty,
                opt => opt
                    .MapFrom(src => Enum.Parse(typeof(TestDifficulty), src.Difficulty)));

        
    }
}