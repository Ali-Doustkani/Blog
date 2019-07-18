using AutoMapper;
using Blog.Domain;
using Blog.Services.Administrator;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Services
{
   public class DraftEntryMappingTests
   {
      public DraftEntryMappingTests()
      {
         var config = new MapperConfiguration(cfg => cfg.AddProfile<PostProfile>());
         _mapper = config.CreateMapper();
      }

      private readonly IMapper _mapper;

      [Fact]
      public void Map_DraftEntry_to_Draft()
      {
         _mapper.Map<Draft>(new DraftEntry
         {
            Content = "content",
            EnglishUrl = "englishUrl",
            Id = 1,
            Language = Language.English,
            PublishDate = new DateTime(2017, 5, 1),
            Summary = "summary",
            Tags = "tags",
            Title = "title"
         })
            .Should()
            .BeEquivalentTo(new Draft
            {
               Content = "content",
               Id = 1,
               Info = new PostInfo
               {
                  EnglishUrl = "englishUrl",
                  Id = 1,
                  Language = Language.English,
                  PublishDate = new DateTime(2017, 5, 1),
                  Summary = "summary",
                  Tags = "tags",
                  Title = "title"
               }
            });
      }
   }
}
