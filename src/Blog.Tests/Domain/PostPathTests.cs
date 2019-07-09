using Blog.Domain;
using Xunit;

namespace Blog.Tests.Domain
{
    public class PostPathTests
    {
        [Fact]
        public void Increment_pure_name() =>
            Assert.Equal("pic-1.jpeg", PostPath.Increment("pic.jpeg"));

        [Fact]
        public void Increment_incremented_name() =>
            Assert.Equal("pic-2.jpeg", PostPath.Increment("pic-1.jpeg"));

        [Fact]
        public void Increment_with_several_dashes() =>
            Assert.Equal("pic-123-2.jpeg", PostPath.Increment("pic-123-1.jpeg"));
    }
}
