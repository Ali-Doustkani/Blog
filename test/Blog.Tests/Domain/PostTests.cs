using Blog.Domain.Blogging;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class PostTests
   {
      [Theory]
      [InlineData(",,a,b")]
      [InlineData("a,,,,c")]
      [InlineData("a,b,")]
      public void TagCollection_IgnoresEmptyStringParts(string tags) =>
         new Post(1, "title", DateTime.Now, Language.English, "summary", tags, "URL", "CONTENT")
         .GetTags().Should().HaveCount(2);

      [Fact]
      public void GetShortPersianDate() =>
         new Post(1, "title", new DateTime(2018, 12, 25), Language.English, "summary", "tags", "URL", "CONTENT")
         .GetShortPersianDate().Should().Be("دی 1397");

      [Fact]
      public void GetLongPersianDate() =>
         new Post(1, "title", new DateTime(2018, 12, 25), Language.English, "summary", "tags", "URL", "CONTENT")
         .GetLongPersianDate().Should().Be("سه شنبه، 4 دی 1397");
   }
}
