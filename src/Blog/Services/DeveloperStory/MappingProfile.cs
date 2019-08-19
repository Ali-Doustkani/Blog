using AutoMapper;
using Blog.Domain.DeveloperStory;

namespace Blog.Services.DeveloperStory
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Developer, DeveloperEntry>()
            .ForMember(
               dest => dest.Experiences,
               o => o.MapFrom(src => src.Experiences))
            .ForMember(
               dest => dest.SideProjects,
               o => o.MapFrom(src => src.SideProjects));

         CreateMap<Experience, ExperienceEntry>()
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")));

         CreateMap<SideProject, SideProjectEntry>();
      }
   }
}
