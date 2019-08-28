using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class DraftTests
   {
      [Fact]
      public void Farsi_posts_should_be_slugified_to_english_url() =>
        new Draft(0, "the post", "the-url", Language.Farsi, null, null, null)
        .Slugify()
        .Should()
        .Be("the-url");

      [Fact]
      public void Throw_if_english_url_is_not_available_for_farsi_posts() =>
         new Draft(0, "the post", null, Language.Farsi, null, null, null)
         .Invoking(x => x.Slugify())
         .Should()
         .Throw<InvalidOperationException>();

      [Fact]
      public void Use_english_url_for_english_posts_if_available() =>
         new Draft(0, "the post", "the-url", Language.English, null, null, null)
         .Slugify()
         .Should()
         .Be("the-url");

      [Theory]
      [InlineData("js intro", "js-intro")]
      [InlineData("js      intro", "js-intro")]
      [InlineData("learn: ASP.NET Core", "learn-aspnet-core")]
      [InlineData("LEARN JS", "learn-js")]
      public void Slugify(string title, string result) =>
         new Draft(0, title, null, Language.English, null, null, null)
         .Slugify()
         .Should()
         .Be(result);

      [Theory]
      [InlineData("c# in depth", "csharp-in-depth")]
      [InlineData("Webpack/node.js////react", "webpack-nodejs-react")]
      [InlineData(@"webpack\node.js\\\\\react", "webpack-nodejs-react")]
      public void Slugify_change_some_characters(string title, string result) =>
         new Draft(0, title, null, Language.English, null, null, null)
         .Slugify()
         .Should()
         .Be(result);

      [Fact]
      public void Dont_publish_when_there_is_no_title()
      {
         var draft = new Draft(0, "", "", Language.English, "read about js", "js", "Learning JS");
         draft.Invoking(d => d.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>(), Mock.Of<IStorageState>()))
            .Should()
            .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_tag()
      {
         var draft = new Draft(0, "JS", null, Language.English, "read about js", "", "Learning JS");
         draft.Invoking(d => d.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>(), Mock.Of<IStorageState>()))
            .Should()
            .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_summary()
      {
         new Draft(0, "JS", null, Language.English, "", "js", "Learning JS")
         .Invoking(d => d.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>(), Mock.Of<IStorageState>()))
         .Should()
         .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_content()
      {
         new Draft(0, "JS", null, Language.English, "learn js", "js", "")
         .Invoking(d => d.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>(), Mock.Of<IStorageState>()))
         .Should()
         .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Set_now_as_post_date_when_publishing_for_the_first_time()
      {
         var dateProvider = new Mock<IDateProvider>();
         dateProvider.Setup(x => x.Now).Returns(new DateTime(2019, 8, 27));
         var htmlProcessor = new Mock<IHtmlProcessor>();
         htmlProcessor.Setup(x => x.Process(It.IsAny<string>())).Returns("<p>TEXT</p>");

         var draft = new Draft(0, "title", null, Language.English, "summary", "tags", "<p>TEXT</p>");

         draft.Publish(dateProvider.Object, htmlProcessor.Object, Mock.Of<IStorageState>())
            .Post
            .PublishDate
            .Should()
            .HaveDay(27)
            .And
            .HaveMonth(8)
            .And
            .HaveYear(2019);

         dateProvider.Setup(x => x.Now).Returns(new DateTime(2001, 1, 1));
         draft.Publish(dateProvider.Object, htmlProcessor.Object, Mock.Of<IStorageState>())
            .Post
            .PublishDate
            .Should()
            .HaveDay(27)
            .And
            .HaveMonth(8)
            .And
            .HaveYear(2019);
      }
   }
}
