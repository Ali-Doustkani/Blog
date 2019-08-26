using AutoMapper;
using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;

namespace Blog.Services.Home
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         CreateMap<Post, PostRow>()
            .ForMember(
                 dest => dest.Date,
                 o => o.MapFrom<ShortDateResolver>())
              .ForMember(
                 dest => dest.Tags,
                 o => o.MapFrom(src => src.GetTags()));

         CreateMap<Post, PostViewModel>()
             .ForMember(
                 dest => dest.Date,
                 o => o.MapFrom<LongDateResolver>())
               .ForMember(
                 dest => dest.Tags,
                 o => o.MapFrom(src => src.GetTags()));

         CreateMap<Developer, DeveloperViewModel>()
            .ForMember(
               dest => dest.Skills,
               o => o.MapFrom(src => src.GetSkillLines()))
            .ForMember(
               dest => dest.Summary,
               o => o.MapFrom(src => src.Summary.Content));

         CreateMap<Experience, ExperienceViewModel>()
            .ForMember(
               dest => dest.Content,
               o => o.MapFrom(src => src.Content.Content))
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.Period.StartDate))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.Period.EndDate));

         CreateMap<SideProject, SideProjectViewModel>()
            .ForMember(
               dest => dest.Content,
               o => o.MapFrom(src => src.Content.Content));

         CreateMap<Education, EducationViewModel>()
            .ForMember(
               dest => dest.StartDate,
               o => o.MapFrom(src => src.Period.StartDate))
            .ForMember(
               dest => dest.EndDate,
               o => o.MapFrom(src => src.Period.EndDate));
      }

      public class ShortDateResolver : IValueResolver<Post, PostRow, string>
      {
         public string Resolve(Post source, PostRow destination, string destMember, ResolutionContext context) =>
              source.Language == Language.English ?
              source.PublishDate.ToString("MMM yyyy") :
              source.GetShortPersianDate();
      }

      public class LongDateResolver : IValueResolver<Post, PostViewModel, string>
      {
         public string Resolve(Post source, PostViewModel destination, string destMember, ResolutionContext context) =>
             source.Language == Language.English ?
             source.PublishDate.ToString("D") :
             source.GetLongPersianDate();
      }
   }
}
