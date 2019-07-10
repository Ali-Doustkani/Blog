using AutoMapper;
using Blog.Domain;
using Blog.Services.Home;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services.Home
{
    [Trait("Category", "Integration")]
    public class ServiceTests
    {
        public ServiceTests()
        {
            _blogContext = Db.CreateInMemory();
            _blogContext.Drafts.Add(new Draft
            {
                Id = 1,
                Content = "<h1>RegularExpressions</h1>"
            });
            _blogContext.Drafts.Add(new Draft
            {
                Id = 2,
                Content = "<h1>Learn React</h1>"
            });
            _blogContext.Drafts.Add(new Draft
            {
                Id = 3,
                Content = "<h1>سی شارپ در 24 سال</h1>"
            });
            _blogContext.Infos.Add(new PostInfo
            {
                Id = 1,
                Language = Language.English,
                PublishDate = new DateTime(2019, 1, 1),
                Summary = "an overview of regex",
                Tags = "C#, Regex",
                Title = "Regular Expressions"
            });
            _blogContext.Infos.Add(new PostInfo
            {
                Id = 2,
                Language = Language.English,
                PublishDate = new DateTime(2018, 1, 1),
                Summary = "learning react in depth",
                Tags = "JS, React",
                Title = "Learn React"
            });
            _blogContext.Infos.Add(new PostInfo
            {
                Id = 3,
                Language = Language.Farsi,
                PublishDate = new DateTime(2017, 1, 1),
                Summary = "آموزش سی شارپ",
                Tags = "C#, .NET, ASP.NET",
                Title = "سی شارپ در 24 سال"
            });
            _blogContext.Posts.Add(new Post
            {
                Content = "<h1>Regular Expressions</h1>",
                Id = 1,
                Url = "Regular-Expressions"
            });
            _blogContext.Posts.Add(new Post
            {
                Content = "<h1>C#</h1>",
                Id = 3,
                Url = "سی-شارپ-در-24-سال"
            });
            _blogContext.SaveChanges();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<PostProfile>());

            _services = new Service(_blogContext, config.CreateMapper());
        }

        private readonly BlogContext _blogContext;
        private readonly Service _services;

        [Fact]
        public void GetPosts_with_english_posts()
        {
            var rows = _services.GetPosts(Language.English);

            Assert.Single(rows);
            Assert.Equal("Jan 2019", rows.First().Date);
            Assert.Equal("an overview of regex", rows.First().Summary);
            Assert.Equal(2, rows.First().Tags.Count());
            Assert.Equal("Regular Expressions", rows.First().Title);
            Assert.Equal("Regular-Expressions", rows.First().Url);
        }

        [Fact]
        public void GetPosts_with_farsi_posts()
        {
            var rows = _services.GetPosts(Language.Farsi);

            Assert.Single(rows);
            Assert.Equal("دی 1395", rows.First().Date);
            Assert.Equal("آموزش سی شارپ", rows.First().Summary);
            Assert.Equal(3, rows.First().Tags.Count());
            Assert.Equal("سی شارپ در 24 سال", rows.First().Title);
            Assert.Equal("سی-شارپ-در-24-سال", rows.First().Url);
        }

        [Fact]
        public void Get_english_post()
        {
            var vm = _services.Get("Regular-Expressions");

            Assert.Equal("<h1>Regular Expressions</h1>", vm.Content);
            Assert.Equal("Tuesday, January 1, 2019", vm.Date);
            Assert.Equal(Language.English, vm.Language);
            Assert.Equal(2, vm.Tags.Count());
            Assert.Equal("Regular Expressions", vm.Title);
        }

        [Fact]
        public void Get_farsi_post()
        {
            var vm = _services.Get("سی-شارپ-در-24-سال");

            Assert.Equal("<h1>C#</h1>", vm.Content);
            Assert.Equal("یکشنبه، 12 دی 1395", vm.Date);
            Assert.Equal(Language.Farsi, vm.Language);
            Assert.Equal(3, vm.Tags.Count());
            Assert.Equal("سی شارپ در 24 سال", vm.Title);
        }
    }
}
