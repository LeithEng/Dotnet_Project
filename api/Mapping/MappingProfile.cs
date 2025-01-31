
using AutoMapper;
using api.Models;
using api.DTOs;
namespace api.Mapping;


public class MappingProfile : Profile
{
    public MappingProfile()
    {

        CreateMap<ReactionDto, Reaction>().ReverseMap();
        CreateMap<HobbyDto, Hobby>().ReverseMap();

        CreateMap<User, GetProfileDto>()
            .ForMember(dest => dest.roles, opt => opt.Ignore());

    }
}
