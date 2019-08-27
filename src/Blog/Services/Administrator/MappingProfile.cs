using AutoMapper;
using Blog.Domain.Blogging;
using System;

namespace Blog.Services.Administrator
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Draft, DraftRow>();

         CreateMap<Tuple<Draft, int>, DraftRow>()
             .ForMember(
                 dest => dest.Published,
                 o => o.MapFrom(src => src.Item2 != -1))
             .IncludeMembers(x => x.Item1);

         CreateMap<Draft, DraftEntry>()
             .ReverseMap();

         CreateMap<Tuple<Draft, int>, DraftEntry>()
             .ForMember(
                 dest => dest.Publish,
                 o => o.MapFrom(src => src.Item2 != -1))
             .IncludeMembers(x => x.Item1);
      }
   }
}
