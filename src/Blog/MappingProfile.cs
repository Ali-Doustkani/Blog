﻿using AutoMapper;
using Blog.CQ.DraftListQuery;
using Blog.CQ.DraftSaveCommand;
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
      }
   }
}
