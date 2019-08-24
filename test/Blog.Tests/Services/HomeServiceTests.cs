using Blog.Domain.Blogging;
using Blog.Domain.DeveloperStory;
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
         _context = new ServiceTestContext<HomeServices>();
         _context.Seed(db =>
         {
            db.Drafts.Add(new Draft
            {
               Id = 1,
               Content = "<h1>RegularExpressions</h1>"
            });
            db.Drafts.Add(new Draft
            {
               Id = 2,
               Content = "<h1>Learn React</h1>"
            });
            db.Drafts.Add(new Draft
            {
               Id = 3,
               Content = "<h1>سی شارپ در 24 سال</h1>"
            });
            db.Infos.Add(new PostInfo
            {
               Id = 1,
               Language = Language.English,
               PublishDate = new DateTime(2019, 1, 1),
               Summary = "an overview of regex",
               Tags = "C#, Regex",
               Title = "Regular Expressions"
            });
            db.Infos.Add(new PostInfo
            {
               Id = 2,
               Language = Language.English,
               PublishDate = new DateTime(2018, 1, 1),
               Summary = "learning react in depth",
               Tags = "JS, React",
               Title = "Learn React"
            });
            db.Infos.Add(new PostInfo
            {
               Id = 3,
               Language = Language.Farsi,
               PublishDate = new DateTime(2017, 1, 1),
               Summary = "آموزش سی شارپ",
               Tags = "C#, .NET, ASP.NET",
               Title = "سی شارپ در 24 سال"
            });
            db.Posts.Add(new Post
            {
               PostContent = new PostContent { Id = 1, Content = "<h1>Regular Expressions</h1>" },
               Id = 1,
               Url = "Regular-Expressions"
            });
            db.Posts.Add(new Post
            {
               PostContent = new PostContent { Id = 3, Content = "<h1>C#</h1>" },
               Id = 3,
               Url = "سی-شارپ-در-24-سال"
            });
         });
      }

      private readonly ServiceTestContext<HomeServices> _context;

      [Fact]
      public void GetPosts_with_english_posts()
      {
         using (var svc = _context.GetService())
         {
            var rows = svc.GetPosts(Language.English);

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
      }

      [Fact]
      public void GetPosts_with_farsi_posts()
      {
         using (var svc = _context.GetService())
         {
            var rows = svc.GetPosts(Language.Farsi);

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
      }

      [Fact]
      public void Get_english_post()
      {
         using (var svc = _context.GetService())
         {
            var vm = svc.Get("Regular-Expressions");

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
      }

      [Fact]
      public void Get_farsi_post()
      {
         using (var svc = _context.GetService())
         {
            var vm = svc.Get("سی-شارپ-در-24-سال");

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

      [Fact]
      public void GetDeveloper()
      {
         using (var db = _context.GetDatabase())
         {
            var developer = new Developer("<p contenteditable=\"true\">Hi I'm <strong>Ali</strong></p>", "C#\nJS");
            developer.AddExperience("Lodgify",
               "C# Developer",
               new DateTime(2018, 1, 1),
               new DateTime(2019, 1, 1),
               "<p contenteditable>worked on web app</p>");
            developer.AddExperience("Parmis",
               "C# Programmer",
               new DateTime(2017, 1, 1),
               new DateTime(2018, 1, 1),
               "<p contenteditable>worked on desktop app</p>");
            developer.AddSideProject("Richtext", "<p contenteditable>HTML Richtext</p>");
            developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         using (var svc = _context.GetService())
         {
            var vm = svc.GetDeveloper();
            vm.Should().BeEquivalentTo(new
            {
               Summary = "<p>Hi I'm <strong>Ali</strong></p>",
               Skills = new[] { "C#", "JS" },
               Experiences = new[]
               {
                  new
                  {
                     Company = "Parmis",
                     Position = "C# Programmer",
                     StartDate = new DateTime(2017,1,1),
                     EndDate = new  DateTime(2018,1,1),
                     Content = "<p>worked on desktop app</p>"
                  },
                  new
                  {
                     Company = "Lodgify",
                     Position = "C# Developer",
                     StartDate = new DateTime(2018,1,1),
                     EndDate = new DateTime(2019,1,1),
                     Content = "<p>worked on web app</p>"
                  }
               },
               SideProjects = new[]
               {
                  new
                  {
                     Title = "Richtext",
                     Content = "<p>HTML Richtext</p>"
                  }
               },
               Educations = new[]
               {
                  new
                  {
                     Degree = "BS",
                     University = "S&C",
                     StartDate = new DateTime(2010,1,1),
                     EndDate = new DateTime(2011,1,1)
                  }
               }
            });
         }
      }
   }
}
