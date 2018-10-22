using Blog.Model;
using System.Linq;
using Xunit;

namespace Blog.Tests.Model
{
    public class PostTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public void TagCollection_WithEmptyString_EmptyCollection(string tags)
        {
            var post = new Post { Tags = tags };

            Assert.Equal(Enumerable.Empty<string>(), post.TagCollection);
        }

        [Theory]
        [InlineData(",,a,b")]
        [InlineData("a,,,,c")]
        [InlineData("a,b,")]
        public void TagCollection_IgnoresEmptyStringParts(string tags)
        {
            var post = new Post { Tags = tags };

            Assert.Equal(2, post.TagCollection.Count());
        }
    }
}
