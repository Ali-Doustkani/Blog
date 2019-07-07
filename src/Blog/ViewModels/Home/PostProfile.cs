using AutoMapper;
using Blog.Domain;

namespace Blog.ViewModels.Home
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostRow>()
                .ForMember(
                    dest => dest.Tags,
                    x => x.MapFrom(src => src.GetTags()));

            CreateMap<Post, PostViewModel>()
                .ForMember(
                    dest => dest.Content,
                    x => x.MapFrom(src => src.Content.DisplayContent))
                .ForMember(
                    dest => dest.Tags,
                    x => x.MapFrom(src => src.GetTags()));
        }
    }
}
