using Blog.Domain;
using System;
using System.Linq;
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

            Assert.Equal(Enumerable.Empty<string>(), post.GetTags());
        }

        [Theory]
        [InlineData(",,a,b")]
        [InlineData("a,,,,c")]
        [InlineData("a,b,")]
        public void TagCollection_IgnoresEmptyStringParts(string tags)
        {
            var post = new PostInfo { Tags = tags };

            Assert.Equal(2, post.GetTags().Count());
        }

        [Fact]
        public void GetShortPersianDate()
        {
            var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

            Assert.Equal("دی 1397", post.GetShortPersianDate());
        }

        [Fact]
        public void GetLongPersianDate()
        {
            var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

            Assert.Equal("سه شنبه، 4 دی 1397", post.GetLongPersianDate());
        }

    }
}
