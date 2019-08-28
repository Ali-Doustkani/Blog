using AutoMapper;
using Blog.CQ.DeveloperQuery;
using Blog.CQ.DraftListQuery;
using Blog.CQ.DraftSaveCommand;
using Blog.CQ.PostListQuery;
using Blog.CQ.PostQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;
using System;

namespace Blog
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

         CreateMap<Draft, DraftItem>();

         CreateMap<DraftSaveCommand, DraftUpdateCommand>();

         CreateMap<Tuple<Draft, int>, DraftItem>()
             .ForMember(
                 dest => dest.Published,
                 o => o.MapFrom(src => src.Item2 != -1))
             .IncludeMembers(x => x.Item1);

         CreateMap<Draft, DraftSaveCommand>()
            .ForMember(x => x.Publish,
            o => o.MapFrom(src => src.Post != null));

         CreateMap<Tuple<Draft, int>, DraftSaveCommand>()
             .ForMember(
                 dest => dest.Publish,
                 o => o.MapFrom(src => src.Item2 != -1))
             .IncludeMembers(x => x.Item1);

         CreateMap<Post, PostItem>()
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
   }

   public class ShortDateResolver : IValueResolver<Post, PostItem, string>
   {
      public string Resolve(Post source, PostItem destination, string destMember, ResolutionContext context) =>
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
