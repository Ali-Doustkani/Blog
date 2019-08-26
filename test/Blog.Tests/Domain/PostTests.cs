using Blog.Domain.Blogging;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class PostTests
   {
      [Fact]
      public void GetShortPersianDate() =>
       new Post { PublishDate = new DateTime(2018, 12, 25) }
       .GetShortPersianDate().Should().Be("دی 1397");

      [Fact]
      public void GetLongPersianDate() =>
         new Post { PublishDate = new DateTime(2018, 12, 25) }
         .GetLongPersianDate().Should().Be("سه شنبه، 4 دی 1397");
   }
}
