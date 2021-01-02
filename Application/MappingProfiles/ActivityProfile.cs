using Application.Activities.DTOs;
using AutoMapper;
using Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Application.MappingProfiles
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<Activity, ActivityDto>();
            CreateMap<UserActivity, AttendeeDto>()
                .ForMember(dest => dest.DisplayName, opt => opt.MapFrom(src => src.AppUser.DisplayName))
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.AppUser.UserName));                
        }        
    }
}
