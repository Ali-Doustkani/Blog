using AutoMapper;
using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;

namespace Blog.Services.Home
{
   public class MappingProfile : Profile
   {
      public MappingProfile()
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
