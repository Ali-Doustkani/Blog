using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
{
   public class DraftTests
   {
      public DraftTests()
      {
         _htmlProcesssor = new Mock<IHtmlProcessor>();
         _htmlProcesssor.Setup(x => x.Process(It.IsAny<string>())).Returns((string html) => html);
         _dateProvider = new Mock<IDateProvider>();
      }

      private Mock<IHtmlProcessor> _htmlProcesssor;
      private Mock<IDateProvider> _dateProvider;

      [Fact]
      public void Farsi_posts_should_be_slugified_to_english_url()
      {
         var draft = new Draft(0, "the post", "the-url", Language.Farsi, "learn js", "js", "<p>text</p>");
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.Url.Should().Be("the-url");
      }

      [Fact]
      public void Throw_if_english_url_is_not_available_for_farsi_posts()
      {
         new Draft(0, "the post", null, Language.Farsi, "learn js", "js", "<p>text</p>")
            .Publish(_dateProvider.Object, _htmlProcesssor.Object)
            .Errors
            .Should()
            .ContainEquivalentOf("EnglishUrl is required for Farsi posts");
      }

      [Fact]
      public void Use_english_url_for_english_posts_if_available()
      {
         var draft = new Draft(0, "the post", "the-url", Language.English, "learn js", "js", "<p>text</p>");
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.Url.Should().Be("the-url");
      }

      [Theory]
      [InlineData("js intro", "js-intro")]
      [InlineData("js      intro", "js-intro")]
      [InlineData("learn: ASP.NET Core", "learn-aspnet-core")]
      [InlineData("LEARN JS", "learn-js")]
      public void Slugify(string title, string result)
      {
         var draft = new Draft(0, title, null, Language.English, "learn js", "js", "<p>text</p>");
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.Url.Should().Be(result);
      }

      [Theory]
      [InlineData("c# in depth", "csharp-in-depth")]
      [InlineData("Webpack/node.js////react", "webpack-nodejs-react")]
      [InlineData(@"webpack\node.js\\\\\react", "webpack-nodejs-react")]
      public void Slugify_change_some_characters(string title, string result)
      {
         var draft = new Draft(0, title, null, Language.English, "learn js", "js", "<p>text</p>");
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.Url.Should().Be(result);
      }

      [Fact]
      public void Dont_publish_when_there_is_no_title()
      {
         new Draft(0, "", "", Language.English, "read about js", "js", "Learning JS")
            .Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>())
            .Errors
            .Should()
            .ContainEquivalentOf("'Title' is required");
      }

      [Fact]
      public void Dont_publish_when_there_is_no_tag()
      {
         new Draft(0, "JS", null, Language.English, "read about js", "", "Learning JS")
            .Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>())
            .Errors
            .Should()
            .ContainEquivalentOf("'Tags' is required");
      }

      [Fact]
      public void Dont_publish_when_there_is_no_summary()
      {
         new Draft(0, "JS", null, Language.English, "", "js", "Learning JS")
            .Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>())
            .Errors
            .Should()
            .ContainEquivalentOf("'Summary' is required");
      }

      [Fact]
      public void Dont_publish_when_there_is_no_content()
      {
         new Draft(0, "JS", null, Language.English, "learn js", "js", "")
            .Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>())
            .Errors
            .Should()
            .ContainEquivalentOf("'Content' is required");
      }

      [Fact]
      public void Set_now_as_post_date_when_publishing_for_the_first_time()
      {
         var dateProvider = new Mock<IDateProvider>();
         dateProvider.Setup(x => x.Now).Returns(new DateTime(2019, 8, 27));
         var htmlProcessor = new Mock<IHtmlProcessor>();
         htmlProcessor.Setup(x => x.Process(It.IsAny<string>())).Returns("<p>TEXT</p>");

         var draft = new Draft(0, "title", null, Language.English, "summary", "tags", "<p>TEXT</p>");

         draft.Publish(dateProvider.Object, htmlProcessor.Object);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2019, 8, 27));

         dateProvider.Setup(x => x.Now).Returns(new DateTime(2001, 1, 1));
         draft.Publish(dateProvider.Object, htmlProcessor.Object);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2019, 8, 27));
      }

      [Fact]
      public void Reset_publish_date_after_unpublishing()
      {
         var draft = new Draft(1, "JS", null, Language.English, "learn js", "js", "<p>text</p>");

         _dateProvider.Setup(x => x.Now).Returns(new DateTime(2010, 1, 1));
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2010, 1, 1));

         _dateProvider.Setup(x => x.Now).Returns(new DateTime(2011, 1, 1));
         draft.Unpublish();
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2011, 1, 1));
      }

      [Fact]
      public void Remove_post_after_unpublishing()
      {
         var draft = new Draft(1, "JS", null, Language.English, "learn js", "js", "<p>text</p>");
         draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         draft.Unpublish();
         draft.Post.Should().BeNull();
      }

      [Fact]
      public void Dont_publish_if_language_of_code_block_is_not_specified()
      {
         var draft = new Draft();
         var command = new DraftUpdateCommand
         {
            Title = "JS",
            Summary = "Learn JS",
            Tags = "js",
            Language = Language.English,
            Content = "<pre class=\"code\">some code</code>"
         };
         draft.Update(command);
         var result = draft.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>());
         result.Failed.Should().BeTrue();
         result.Errors.Should().ContainEquivalentOf("Language is not specified for the code block #1");
      }

      [Fact]
      public void Publish_if_code_block_is_empty()
      {
         var draft = new Draft();
         var command = new DraftUpdateCommand
         {
            Title = "JS",
            Summary = "Learn JS",
            Tags = "js",
            Language = Language.English,
            Content = "<p>text</p><pre class=\"code\"> </code>"
         };
         draft.Update(command);
         var result = draft.Publish(_dateProvider.Object, _htmlProcesssor.Object);
         result.Failed.Should().BeFalse();
      }

      [Fact]
      public void Dont_publish_if_code_block_language_is_invalid()
      {
         var draft = new Draft();
         var command = new DraftUpdateCommand
         {
            Title = "JS",
            Summary = "Learn JS",
            Tags = "js",
            Language = Language.English,
            Content = "<pre class=\"code\">\nclojure\nsome code</code>"
         };
         draft.Update(command);
         var result = draft.Publish(Mock.Of<IDateProvider>(), Mock.Of<IHtmlProcessor>());
         result.Failed.Should().BeTrue();

         result.Errors
            .First()
            .Should()
            .StartWith("Specified language in code block #1 is not valid");
      }

      [Fact]
      public void Update_values()
      {
         var draft = new Draft();
         var command = new DraftUpdateCommand
         {
            Title = "JS",
            Summary = "Learn JS",
            Tags = "ts, js",
            Language = Language.English,
            Content = "<p>text</p>"
         };
         draft.Update(command);

         draft.Title.Should().Be("JS");
         draft.Summary.Should().Be("Learn JS");
         draft.Tags.Should().Be("ts, js");
         draft.Language.Should().Be(Language.English);
         draft.Content.Should().Be("<p>text</p>");
      }
   }
}
