using AutoMapper;
using Blog.CQ.PostQuery;
using Blog.CQ.PreviewQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Handler = Blog.CQ.PreviewQuery.Handler;

namespace Blog.Tests.CQ
{
   public class DraftPreviewQueryTests
   {
      public DraftPreviewQueryTests()
      {
         _dateProvider = new Mock<IDateProvider>();
         _htmlProcessor = new HtmlProcessor(Mock.Of<ICodeFormatter>(), Mock.Of<IImageProcessor>());
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(_dateProvider.Object, _htmlProcessor, mapper);
      }

      private readonly IRequestHandler<DraftPreviewQuery, PostViewModel> _handler;
      private readonly Mock<IDateProvider> _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;

      [Fact]
      public async Task Make_preview()
      {
         _dateProvider.Setup(x => x.Now).Returns(new DateTime(2010, 1, 1));

         var result = await _handler.Handle(new DraftPreviewQuery
         {
            Title = "JS",
            Content = "<p contenteditable=\"true\">text</p>",
            Language = Language.English,
            Tags = "js, es"
         }, default);

         result.Should().BeEquivalentTo(new PostViewModel
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
