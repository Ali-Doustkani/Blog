using Blog.Domain.Blogging;
using FluentAssertions;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Domain.Tests.Blogging
{
   public class DraftTests
   {
      public DraftTests()
      {
         _htmlProcesssor = Substitute.For<IHtmlProcessor>(); ;
         _htmlProcesssor.ProcessAsync(Arg.Any<string>()).Returns(x => Task.FromResult((string)x[0]));
         _dateProvider = Substitute.For<IDateProvider>();
      }

      private IHtmlProcessor _htmlProcesssor;
      private IDateProvider _dateProvider;

      [Fact]
      public async Task Farsi_posts_should_be_slugified_to_english_url()
      {
         var draft = new Draft(0, "the post", "the-url", Language.Farsi, "learn js", "js", "<p>text</p>");
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.Url.Should().Be("the-url");
      }

      [Fact]
      public async Task Throw_if_english_url_is_not_available_for_farsi_posts()
      {
         var draft = new Draft(0, "the post", null, Language.Farsi, "learn js", "js", "<p>text</p>");
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Errors.Should().ContainEquivalentOf("EnglishUrl is required for Farsi posts");
      }

      [Fact]
      public async Task Use_english_url_for_english_posts_if_available()
      {
         var draft = new Draft(0, "the post", "the-url", Language.English, "learn js", "js", "<p>text</p>");
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.Url.Should().Be("the-url");
      }

      [Theory]
      [InlineData("js intro", "js-intro")]
      [InlineData("js      intro", "js-intro")]
      [InlineData("learn: ASP.NET Core", "learn-aspnet-core")]
      [InlineData("LEARN JS", "learn-js")]
      public async Task Slugify(string title, string result)
      {
         var draft = new Draft(0, title, null, Language.English, "learn js", "js", "<p>text</p>");
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.Url.Should().Be(result);
      }

      [Theory]
      [InlineData("c# in depth", "csharp-in-depth")]
      [InlineData("Webpack/node.js////react", "webpack-nodejs-react")]
      [InlineData(@"webpack\node.js\\\\\react", "webpack-nodejs-react")]
      public async Task Slugify_change_some_characters(string title, string result)
      {
         var draft = new Draft(0, title, null, Language.English, "learn js", "js", "<p>text</p>");
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.Url.Should().Be(result);
      }

      [Fact]
      public async Task Dont_publish_when_there_is_no_title()
      {
         var draft = new Draft(0, "", "", Language.English, "read about js", "js", "Learning JS");
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Errors.Should().ContainEquivalentOf("'Title' is required");
      }

      [Fact]
      public async Task Dont_publish_when_there_is_no_tag()
      {
         var draft = new Draft(0, "JS", null, Language.English, "read about js", "", "Learning JS");
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Errors.Should().ContainEquivalentOf("'Tags' is required");
      }

      [Fact]
      public async Task Dont_publish_when_there_is_no_summary()
      {
         var draft = new Draft(0, "JS", null, Language.English, "", "js", "Learning JS");
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Errors.Should().ContainEquivalentOf("'Summary' is required");
      }

      [Fact]
      public async Task Dont_publish_when_there_is_no_content()
      {
         var draft = new Draft(0, "JS", null, Language.English, "learn js", "js", "");
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Errors.Should().ContainEquivalentOf("'Content' is required");
      }

      [Fact]
      public async Task Set_now_as_post_date_when_publishing_for_the_first_time()
      {
         var dateProvider = _dateProvider;
         dateProvider.Now.Returns(new DateTime(2019, 8, 27));
         var htmlProcessor = _htmlProcesssor;
         htmlProcessor.ProcessAsync(Arg.Any<string>()).Returns("<p>TEXT</p>");

         var draft = new Draft(0, "title", null, Language.English, "summary", "tags", "<p>TEXT</p>");

         await draft.Publish(dateProvider, htmlProcessor);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2019, 8, 27));

         dateProvider.Now.Returns(new DateTime(2001, 1, 1));
         await draft.Publish(dateProvider, htmlProcessor);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2019, 8, 27));
      }

      [Fact]
      public async Task Reset_publish_date_after_unpublishing()
      {
         var draft = new Draft(1, "JS", null, Language.English, "learn js", "js", "<p>text</p>");

         _dateProvider.Now.Returns(new DateTime(2010, 1, 1));
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2010, 1, 1));

         _dateProvider.Now.Returns(new DateTime(2011, 1, 1));
         draft.Unpublish();
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Post.PublishDate.Should().BeSameDateAs(new DateTime(2011, 1, 1));
      }

      [Fact]
      public async Task Remove_post_after_unpublishing()
      {
         var draft = new Draft(1, "JS", null, Language.English, "learn js", "js", "<p>text</p>");
         await draft.Publish(_dateProvider, _htmlProcesssor);
         draft.Unpublish();
         draft.Post.Should().BeNull();
      }

      [Fact]
      public async Task Dont_publish_if_language_of_code_block_is_not_specified()
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
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Failed.Should().BeTrue();
         result.Errors.Should().ContainEquivalentOf("Language is not specified for the code block #1");
      }

      [Fact]
      public async Task Publish_if_code_block_is_empty()
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
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
         result.Failed.Should().BeFalse();
      }

      [Fact]
      public async Task Dont_publish_if_code_block_language_is_invalid()
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
         var result = await draft.Publish(_dateProvider, _htmlProcesssor);
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
