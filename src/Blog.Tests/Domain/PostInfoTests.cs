using Blog.Domain;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class PostInfoTests
   {
      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData("   ")]
      public void TagCollection_WithEmptyString_EmptyCollection(string tags)
      {
         var post = new PostInfo { Tags = tags };

         post.GetTags().Should().BeEmpty();
      }

      [Theory]
      [InlineData(",,a,b")]
      [InlineData("a,,,,c")]
      [InlineData("a,b,")]
      public void TagCollection_IgnoresEmptyStringParts(string tags)
      {
         var post = new PostInfo { Tags = tags };

         post.GetTags().Should().HaveCount(2);
      }

      [Fact]
      public void GetShortPersianDate()
      {
         var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

         post.GetShortPersianDate().Should().Be("دی 1397");
      }

      [Fact]
      public void GetLongPersianDate()
      {
         var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

         post.GetLongPersianDate().Should().Be("سه شنبه، 4 دی 1397");
      }

      [Fact]
      public void Farsi_posts_should_be_slugified_to_english_url() =>
         new PostInfo { Language = Language.Farsi, EnglishUrl = "the-url" }
         .Slugify()
         .Should()
         .Be("the-url");

      [Theory]
      [InlineData("js intro", "js-intro")]
      [InlineData("js      intro", "js-intro")]
      [InlineData("learn: ASP.NET Core", "learn-aspnet-core")]
      [InlineData("LEARN JS", "learn-js")]
      public void Slugify(string title, string result) =>
         new PostInfo { Title = title }
         .Slugify()
         .Should()
         .Be(result);

      [Theory]
      [InlineData("c# in depth", "csharp-in-depth")]
      [InlineData("Webpack/node.js////react", "webpack-nodejs-react")]
      [InlineData(@"webpack\node.js\\\\\react", "webpack-nodejs-react")]
      public void Slugify_change_some_characters(string title, string result) =>
         new PostInfo { Title = title }
         .Slugify()
         .Should()
         .Be(result);
   }
}
