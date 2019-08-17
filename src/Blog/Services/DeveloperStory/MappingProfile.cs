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
               o => o.MapFrom(src => src.SideProjects))
            .ReverseMap();

         CreateMap<Experience, ExperienceEntry>()
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.StartDate.ToString("yyyy-MM-dd")))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.EndDate.ToString("yyyy-MM-dd")))
            .ReverseMap()
            .ForMember(
               dest => dest.Id,
               o => o.MapFrom(src => ToInt(src.Id)));

         CreateMap<SideProject, SideProjectEntry>()
            .ReverseMap()
            .ForMember(
               dest => dest.Id,
               o => o.MapFrom(src => ToInt(src.Id)));
      }

      private int ToInt(string id)
      {
         int result;
         int.TryParse(id, out result);
         return result;
      }
   }



   public class IdResolver : IValueResolver<ExperienceEntry, Experience, int>
   {
      public int Resolve(ExperienceEntry source, Experience destination, int destMember, ResolutionContext context)
      {
         throw new System.NotImplementedException();
      }
   }
}
