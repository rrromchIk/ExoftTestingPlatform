﻿using AutoMapper;
using Security.Dto;
using Security.Models;

namespace Security.Helpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserSignUpDto, ApplicationUser>();
        CreateMap<UserSignUpDto, UserResponseDto>();
    }
}