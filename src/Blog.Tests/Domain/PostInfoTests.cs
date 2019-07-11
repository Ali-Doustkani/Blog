﻿using Blog.Domain;
using FluentAssertions;
using System;
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

            post.GetTags().Should().BeEmpty();
        }

        [Theory]
        [InlineData(",,a,b")]
        [InlineData("a,,,,c")]
        [InlineData("a,b,")]
        public void TagCollection_IgnoresEmptyStringParts(string tags)
        {
            var post = new PostInfo { Tags = tags };

            post.GetTags().Should().HaveCount(2);
        }

        [Fact]
        public void GetShortPersianDate()
        {
            var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

            post.GetShortPersianDate().Should().Be("دی 1397");
        }

        [Fact]
        public void GetLongPersianDate()
        {
            var post = new PostInfo { PublishDate = new DateTime(2018, 12, 25) };

            post.GetLongPersianDate().Should().Be("سه شنبه، 4 دی 1397");
        }

        [Theory]
        [InlineData("پیش مقدمه", "پیش-مقدمه")]
        [InlineData("پیش      مقدمه", "پیش-مقدمه")]
        [InlineData("یادگیری: ASP.NET Core", "یادگیری-ASP-NET-Core")]
        public void EncodeTitle(string title, string result)
        {
            var info = new PostInfo { Title = title };
            info.Slugify().Should().Be(result);
        }
    }
}
