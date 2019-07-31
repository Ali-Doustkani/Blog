using AutoMapper;
using Blog.Domain.Blogging;
using System;

namespace Blog.Services.Administrator
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
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

         CreateMap<Tuple<Draft, int>, DraftEntry>()
             .ForMember(
                 dest => dest.Publish,
                 o => o.MapFrom(src => src.Item2 != -1))
             .IncludeMembers(x => x.Item1);
      }
   }
}
