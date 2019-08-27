﻿using Blog.Domain.Blogging;
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
        new Draft() { Title = "the post", Language = Language.Farsi, EnglishUrl = "the-url" }
        .Slugify()
        .Should()
        .Be("the-url");

      [Fact]
      public void Throw_if_english_url_is_not_available_for_farsi_posts() =>
         new Draft() { Title = "the post", Language = Language.Farsi }
         .Invoking(x => x.Slugify())
         .Should()
         .Throw<InvalidOperationException>();

      [Fact]
      public void Use_english_url_for_english_posts_if_available() =>
         new Draft() { Title = "the post", Language = Language.English, EnglishUrl = "the-url" }
         .Slugify()
         .Should()
         .Be("the-url");

      [Theory]
      [InlineData("js intro", "js-intro")]
      [InlineData("js      intro", "js-intro")]
      [InlineData("learn: ASP.NET Core", "learn-aspnet-core")]
      [InlineData("LEARN JS", "learn-js")]
      public void Slugify(string title, string result) =>
         new Draft() { Title = title }
         .Slugify()
         .Should()
         .Be(result);

      [Theory]
      [InlineData("c# in depth", "csharp-in-depth")]
      [InlineData("Webpack/node.js////react", "webpack-nodejs-react")]
      [InlineData(@"webpack\node.js\\\\\react", "webpack-nodejs-react")]
      public void Slugify_change_some_characters(string title, string result) =>
         new Draft() { Title = title }
         .Slugify()
         .Should()
         .Be(result);

      [Fact]
      public void Dont_publish_when_there_is_no_title()
      {
         var draft = new Draft
         {
            Title = "",
            Tags = "js",
            Summary = "read about js",
            Language = Language.English,
            Content = "Learning JS"
         };
         draft.Invoking(d => d.Publish(DateTime.Now, Mock.Of<IHtmlProcessor>()))
            .Should()
            .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_tag()
      {
         var draft = new Draft
         {
            Title = "JS",
            Tags = "",
            Summary = "read about js",
            Language = Language.English,
            Content = "Learning JS"
         };
         draft.Invoking(d => d.Publish(DateTime.Now, Mock.Of<IHtmlProcessor>()))
            .Should()
            .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_summary()
      {
         new Draft
         {
            Title = "JS",
            Tags = "js",
            Summary = "",
            Language = Language.English,
            Content = "Learning JS"
         }.Invoking(d => d.Publish(DateTime.Now, Mock.Of<IHtmlProcessor>()))
         .Should()
         .Throw<InvalidOperationException>();
      }

      [Fact]
      public void Dont_publish_when_there_is_no_content()
      {
         new Draft
         {
            Title = "JS",
            Tags = "js",
            Summary = "learn js",
            Language = Language.English,
            Content = ""
         }.Invoking(d => d.Publish(DateTime.Now, Mock.Of<IHtmlProcessor>()))
         .Should()
         .Throw<InvalidOperationException>();
      }
   }
}
