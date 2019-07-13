using AutoMapper;
using Blog.Domain;

namespace Blog.Services.Home
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostInfo, PostRow>()
                .ForMember(
                    dest => dest.Tags,
                    o => o.MapFrom(src => src.GetTags()))
                .ForMember(
                    dest => dest.Date,
                    o => o.MapFrom<ShortDateResolver>());

            CreateMap<Post, PostRow>()
                .IncludeMembers(x => x.Info);

            CreateMap<PostInfo, PostViewModel>()
                .ForMember(
                    dest => dest.Tags,
                    o => o.MapFrom(src => src.GetTags()))
                .ForMember(
                    dest => dest.Date,
                    o => o.MapFrom<LongDateResolver>());

            CreateMap<Post, PostViewModel>()
                .ForMember(
                    dest => dest.Content, o => o.MapFrom(src => src.PostContent.Content))
                .IncludeMembers(x => x.Info);
        }

        public class ShortDateResolver : IValueResolver<PostInfo, PostRow, string>
        {
            public string Resolve(PostInfo source, PostRow destination, string destMember, ResolutionContext context) =>
                 source.Language == Language.English ?
                 source.PublishDate.ToString("MMM yyyy") :
                 source.GetShortPersianDate();

        }

        public class LongDateResolver : IValueResolver<PostInfo, PostViewModel, string>
        {
            public string Resolve(PostInfo source, PostViewModel destination, string destMember, ResolutionContext context) =>
                source.Language == Language.English ?
                source.PublishDate.ToString("D") :
                source.GetLongPersianDate();
        }
    }
}
