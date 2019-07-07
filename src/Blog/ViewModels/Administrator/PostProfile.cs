using AutoMapper;
using Blog.Domain;

namespace Blog.ViewModels.Administrator
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostEntry, Post>()
                .ForMember(
                    dest => dest.MarkedContent,
                    x => x.MapFrom(src => src.Content));
        }
    }
}
