using Blog.Domain;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
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

            Assert.Equal(Enumerable.Empty<string>(), post.GetTags());
        }

        [Theory]
        [InlineData(",,a,b")]
        [InlineData("a,,,,c")]
        [InlineData("a,b,")]
        public void TagCollection_IgnoresEmptyStringParts(string tags)
        {
            var post = new Post { Tags = tags };

            Assert.Equal(2, post.GetTags().Count());
        }

        [Fact]
        public void GetShortPersianDate()
        {
            var post = new Post { PublishDate = new DateTime(2018, 12, 25) };

            Assert.Equal("دی 1397", post.GetShortPersianDate());
        }

        [Fact]
        public void GetLongPersianDate()
        {
            var post = new Post { PublishDate = new DateTime(2018, 12, 25) };

            Assert.Equal("سه شنبه، 4 دی 1397", post.GetLongPersianDate());
        }

        [Theory]
        [InlineData("پیش مقدمه", "پیش-مقدمه")]
        [InlineData("پیش      مقدمه", "پیش-مقدمه")]
        [InlineData("یادگیری: ASP.NET Core", "یادگیری-ASP-NET-Core")]
        public void PopulateUrlTitle(string title, string result)
        {
            var post = new Post { Title = title };
            post.PopulateUrlTitle();

            Assert.Equal(result, post.UrlTitle);
        }
    }
}
