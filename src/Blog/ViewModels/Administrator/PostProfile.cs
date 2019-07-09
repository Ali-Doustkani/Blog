using AutoMapper;
using Blog.Domain;

namespace Blog.ViewModels.Administrator
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Draft, PostRow>();
            CreateMap<PostEntry, Draft>()
                .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content))
                .ReverseMap();
        }
    }
}
