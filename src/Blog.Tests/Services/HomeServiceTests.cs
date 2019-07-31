using AutoMapper;
using Blog.Domain.Blogging;
using Blog.Services.Home;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services.Home
{
   [Trait("Category", "Integration")]
   public class HomeServiceTests
   {
      public HomeServiceTests()
      {
         var context = Db.CreateInMemory();
         context.Drafts.Add(new Draft
         {
            Id = 1,
            Content = "<h1>RegularExpressions</h1>"
         });
         context.Drafts.Add(new Draft
         {
            Id = 2,
            Content = "<h1>Learn React</h1>"
         });
         context.Drafts.Add(new Draft
         {
            Id = 3,
            Content = "<h1>سی شارپ در 24 سال</h1>"
         });
         context.Infos.Add(new PostInfo
         {
            Id = 1,
            Language = Language.English,
            PublishDate = new DateTime(2019, 1, 1),
            Summary = "an overview of regex",
            Tags = "C#, Regex",
            Title = "Regular Expressions"
         });
         context.Infos.Add(new PostInfo
         {
            Id = 2,
            Language = Language.English,
            PublishDate = new DateTime(2018, 1, 1),
            Summary = "learning react in depth",
            Tags = "JS, React",
            Title = "Learn React"
         });
         context.Infos.Add(new PostInfo
         {
            Id = 3,
            Language = Language.Farsi,
            PublishDate = new DateTime(2017, 1, 1),
            Summary = "آموزش سی شارپ",
            Tags = "C#, .NET, ASP.NET",
            Title = "سی شارپ در 24 سال"
         });
         context.Posts.Add(new Post
         {
            PostContent = new PostContent { Id = 1, Content = "<h1>Regular Expressions</h1>" },
            Id = 1,
            Url = "Regular-Expressions"
         });
         context.Posts.Add(new Post
         {
            PostContent = new PostContent { Id = 3, Content = "<h1>C#</h1>" },
            Id = 3,
            Url = "سی-شارپ-در-24-سال"
         });
         context.SaveChanges();

         var config = new MapperConfiguration(cfg => cfg.AddProfile<PostProfile>());

         _services = new HomeServices(context, config.CreateMapper());
      }

      private readonly HomeServices _services;

      [Fact]
      public void GetPosts_with_english_posts()
      {
         var rows = _services.GetPosts(Language.English);

         rows.Should()
             .HaveCount(1);

         rows.First()
             .Should()
             .BeEquivalentTo(new
             {
                Date = "Jan 2019",
                Summary = "an overview of regex",
                Title = "Regular Expressions",
                Url = "Regular-Expressions",
                Tags = new[] { "C#", "Regex" }
             });
      }

      [Fact]
      public void GetPosts_with_farsi_posts()
      {
         var rows = _services.GetPosts(Language.Farsi);

         rows.Should()
             .HaveCount(1);
         rows.First()
             .Should()
             .BeEquivalentTo(new
             {
                Date = "دی 1395",
                Summary = "آموزش سی شارپ",
                Tags = new[] { "C#", ".NET", "ASP.NET" },
                Title = "سی شارپ در 24 سال",
                Url = "سی-شارپ-در-24-سال"
             });
      }

      [Fact]
      public void Get_english_post()
      {
         var vm = _services.Get("Regular-Expressions");

         vm.Should()
             .BeEquivalentTo(new
             {
                Content = "<h1>Regular Expressions</h1>",
                Date = "Tuesday, January 1, 2019",
                Language = Language.English,
                Tags = new[] { "C#", "Regex" },
                Title = "Regular Expressions"
             });
      }

      [Fact]
      public void Get_farsi_post()
      {
         var vm = _services.Get("سی-شارپ-در-24-سال");

         vm.Should()
             .BeEquivalentTo(new
             {
                Content = "<h1>C#</h1>",
                Date = "یکشنبه، 12 دی 1395",
                Language = Language.Farsi,
                Tags = new[] { "C#", ".NET", "ASP.NET" },
                Title = "سی شارپ در 24 سال"
             });
      }
   }
}
