using AutoMapper;
using api.Models;
using api.DTOs;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ReactionDto, Reaction>().ReverseMap();
    }
}
