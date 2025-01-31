using AutoMapper;
using api.Models;
using api.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<PostDto, Post>().ReverseMap();
        CreateMap<EventDto, Event>().ReverseMap();
        CreateMap<EventDto, Comment>().ReverseMap();

    }
}
