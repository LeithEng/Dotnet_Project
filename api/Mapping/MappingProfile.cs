using AutoMapper;
using api.Models;
using api.DTOs;
using static System.Runtime.InteropServices.JavaScript.JSType;
namespace api.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
         CreateMap<ReactionDto, Reaction>().ReverseMap();
        CreateMap<HobbyDto, Hobby>().ReverseMap();

        CreateMap<User, GetProfileDto>()
            .ForMember(dest => dest.roles, opt => opt.Ignore());
        CreateMap<FavoriteHobbyDto,FavoriteHobby>().ReverseMap();

        CreateMap<PostDto, Post>()
             .ForMember(dest => dest.Id, opt => opt.Ignore())         // Prevent ID from being changed
             .ForMember(dest => dest.UserId, opt => opt.Ignore())     // Prevent UserId from being changed
             .ForMember(dest => dest.User, opt => opt.Ignore())       // Prevent User from being changed
             .ForMember(dest => dest.Comments, opt => opt.Ignore())   // Prevent Comments from being changed
             .ForMember(dest => dest.Reactions, opt => opt.Ignore())  // Prevent Reactions from being changed

             // Only update if the field is not null or empty
             .ForMember(dest => dest.Content, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Content)))
             .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => !string.IsNullOrEmpty(src.ImageUrl)))
             .ForMember(dest => dest.HobbyId, opt => opt.Condition(src => !string.IsNullOrEmpty(src.HobbyId)))
             .ForMember(dest => dest.Hobby, opt => opt.Ignore()); // Hobby should be assigned separately if needed

        CreateMap<Post, PostDto>();


        CreateMap<CommentDto, Comment>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())           // Prevent ID from being changed
            .ForMember(dest => dest.UserId, opt => opt.Ignore())       // Prevent UserId from being changed
            .ForMember(dest => dest.User, opt => opt.Ignore())         // Prevent User from being changed
            .ForMember(dest => dest.Post, opt => opt.Ignore())         // Prevent Post from being changed
            .ForMember(dest => dest.PostId, opt => opt.Ignore())       // Prevent PostId from being changed
                                                                       // Only update if the field is not null or empty

            .ForMember(dest => dest.Content, opt => opt.Condition(src => !string.IsNullOrEmpty(src.Content)));

        CreateMap<Comment, CommentDto>();




        CreateMap<Event, EventDto>();
        CreateMap<EventDto, Event>();

        // Map Event to EventDto
        //  CreateMap<Event, EventDto>()
        //.ForMember(dest => dest.UserEvents, opt => opt.Ignore()); // Ignore UserEvents











    }

}

