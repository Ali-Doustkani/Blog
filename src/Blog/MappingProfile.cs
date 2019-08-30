using AutoMapper;
using Blog.Services.DeveloperQuery;
using Blog.Services.DeveloperSaveCommand;
using Blog.Services.DraftSaveCommand;
using Blog.Services.PostQuery;
using Blog.Services.PreviewQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;

namespace Blog
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
      {
         ConfigureBlogging();
         ConfigureDeveloper();
      }

      private void ConfigureDeveloper()
      {
         CreateMap<DeveloperSaveCommand, DeveloperUpdateCommand>();

         CreateMap<Developer, DeveloperSaveCommand>()
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

      private void ConfigureBlogging()
      {
         CreateMap<DraftSaveCommand, DraftUpdateCommand>();

         CreateMap<DraftPreviewQuery, DraftUpdateCommand>();

         CreateMap<Post, PostViewModel>()
            .ForMember(
               dest => dest.Date,
               o => o.MapFrom<LongDateResolver>())
            .ForMember(
               dest => dest.Tags,
               o => o.MapFrom(src => src.GetTags()));
      }
   }

   public class LongDateResolver : IValueResolver<Post, PostViewModel, string>
   {
      public string Resolve(Post source, PostViewModel destination, string destMember, ResolutionContext context) =>
          source.Language == Language.English ?
          source.PublishDate.ToString("D") :
          source.GetLongPersianDate();
   }
}
