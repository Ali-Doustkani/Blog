using Blog.Domain;
using Xunit;
using FluentAssertions;

namespace Blog.Tests.Domain
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
