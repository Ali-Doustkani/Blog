using AutoMapper;
using Blog.Domain;

namespace Blog.ViewModels.Administrator
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostRow>();
            CreateMap<PostEntry, Post>()
                .ForPath(dest => dest.Content.MarkedContent, opt => opt.MapFrom(src => src.Content))
                .ForPath(dest => dest.Content.Id, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
        }
    }
}
