using AutoMapper;
using Blog.Domain.DeveloperStory;

namespace Blog.Services.DeveloperStory
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Developer, DeveloperUpdateCommand>()
            .ForMember(
               dest => dest.Summary,
               o => o.MapFrom(src => src.Summary.RawContent));

         CreateMap<Experience, ExperienceEntry>()
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.Period.StartDate.ToString("yyyy-MM-dd")))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.Period.EndDate.ToString("yyyy-MM-dd")))
             .ForMember(
               dest => dest.Content,
               o => o.MapFrom(src => src.Content.RawContent));

         CreateMap<SideProject, SideProjectEntry>()
            .ForMember(
               dest => dest.Content,
               o => o.MapFrom(src => src.Content.RawContent));

         CreateMap<Education, EducationEntry>()
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.Period.StartDate.ToString("yyyy-MM-dd")))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.Period.EndDate.ToString("yyyy-MM-dd")));
      }
   }
}
