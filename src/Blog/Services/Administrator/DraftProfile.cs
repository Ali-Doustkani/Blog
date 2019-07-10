using AutoMapper;
using Blog.Domain;
using System;

namespace Blog.Services.Administrator
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<PostInfo, DraftRow>();
            CreateMap<Tuple<PostInfo, int>, DraftRow>()
                .ForMember(
                    dest => dest.Published,
                    o => o.MapFrom(src => src.Item2 != -1))
                .IncludeMembers(x => x.Item1);

            CreateMap<PostInfo, DraftEntry>()
                .ReverseMap();
            CreateMap<Draft, DraftEntry>()
                .IncludeMembers(x => x.Info)
                .ReverseMap();
        }
    }
}
