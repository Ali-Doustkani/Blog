using Blog.Domain.Blogging;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class PostTests
   {
      [Theory]
      [InlineData(null)]
      [InlineData("")]
      [InlineData("   ")]
      public void TagCollection_WithEmptyString_EmptyCollection(string tags) =>
        new Post(DateTime.Now) { Tags = tags }
        .GetTags().Should().BeEmpty();

      [Theory]
      [InlineData(",,a,b")]
      [InlineData("a,,,,c")]
      [InlineData("a,b,")]
      public void TagCollection_IgnoresEmptyStringParts(string tags) =>
         new Post(DateTime.Now) { Tags = tags }
         .GetTags().Should().HaveCount(2);

      [Fact]
      public void GetShortPersianDate() =>
       new Post(new DateTime(2018, 12, 25))
       .GetShortPersianDate().Should().Be("دی 1397");

      [Fact]
      public void GetLongPersianDate() =>
         new Post(new DateTime(2018, 12, 25))
         .GetLongPersianDate().Should().Be("سه شنبه، 4 دی 1397");
   }
}
