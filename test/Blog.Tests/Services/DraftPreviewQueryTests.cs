using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Domain.Blogging.Abstractions;
using Blog.Services.DraftPreviewQuery;
using Blog.Services.PostQuery;
using FluentAssertions;
using MediatR;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;
using Handler = Blog.Services.DraftPreviewQuery.Handler;

namespace Blog.Tests.Services
{
   public class DraftPreviewQueryTests
   {
      public DraftPreviewQueryTests()
      {
         _dateProvider = Substitute.For<IDateProvider>();
         _htmlProcessor = new HtmlProcessor(Substitute.For<ICodeFormatter>(), Substitute.For<IImageProcessor>());
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(_dateProvider, _htmlProcessor, mapper);
      }

      private readonly IRequestHandler<DraftPreviewQuery, Result> _handler;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;

      [Fact]
      public async Task Make_preview()
      {
         _dateProvider.Now.Returns(new DateTime(2010, 1, 1));

         var result = await _handler.Handle(new DraftPreviewQuery
         {
            Title = "JS",
            Content = "<p contenteditable=\"true\">text</p>",
            Language = Language.English,
            Tags = "js, es"
         }, default);

         result.Failed.Should().BeFalse();
         result.Post.Should().BeEquivalentTo(new PostViewModel
         {
            Id = 0,
            Title = "JS",
            Tags = new[] { "js", "es" },
            Date = "Friday, January 1, 2010",
            Content = "<p>text</p>",
            Language = Language.English,
         });
      }

   }
}
