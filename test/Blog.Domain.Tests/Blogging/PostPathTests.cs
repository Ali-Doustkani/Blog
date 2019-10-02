using Blog.Domain.Blogging;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.Blogging
{
   public class PostPathTests
   {
      [Fact]
      public void Increment_pure_name() =>
          PostPath.Increment("pic.jpeg").Should().Be("pic-1.jpeg");

      [Fact]
      public void Increment_incremented_name() =>
          PostPath.Increment("pic-1.jpeg").Should().Be("pic-2.jpeg");

      [Fact]
      public void Increment_with_several_dashes() =>
          PostPath.Increment("pic-123-1.jpeg").Should().Be("pic-123-2.jpeg");
   }
}
