using AutoMapper;
using Blog.Domain;

namespace Blog.ViewModels.Home
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Draft, PostRow>()
                .ForMember(
                    dest => dest.Tags,
                    x => x.MapFrom(src => src.Info.GetTags()));

            CreateMap<Draft, PostViewModel>()
                .ForMember(
                    dest => dest.Content,
                    x => x.MapFrom(src => src.DisplayContent))
                .ForMember(
                    dest => dest.Tags,
                    x => x.MapFrom(src => src.Info.GetTags()));
        }
    }
}
