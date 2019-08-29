//using Blog.Domain;
//using Blog.Domain.Blogging;
//using Blog.Services.Administrator;
//using Blog.Utils;
//using FluentAssertions;
//using Moq;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using Xunit;

//namespace Blog.Tests.Services
//{
//   [Trait("Category", "Integration")]
//   public class AdminiServiceTests
//   {
//      public AdminiServiceTests()
//      {
//         _context = new ServiceTestContext<AdminServices>();
//         _context.Seed(db =>
//         {
//            db.Drafts.Add(new Draft(1, "Javascript FP", "", Language.English, "Learning FP in Javascript", "JS, FP, Node.js", "<p>JS Functional Programming</p>"));
//            db.Drafts.Add(new Draft(2, "Object Oriented C#", "", Language.English, "Learning OOP in C#", "OOP, C#", "<p>Object Oriented C#</p>"));
//            db.Drafts.Add(new Draft(3, "جاوا و ویندوز", "java-windows", Language.Farsi, "استفاده از جاوا در ویندوز", "Java", "<p>جاوا و ویندوز</p>"));
//            db.Posts.Add(new Post(1,
//               "Javascript FP",
//               new DateTime(2019, 1, 1),
//               Language.English,
//               "Learning FP in Javascript",
//              "JS, FP, Node.js",
//              "Javascript-FP",
//              "<p>JS Functional Programming</p>"));
//            db.Posts.Add(new Post(2,
//               "Object Oriented C#",
//               new DateTime(2019, 7, 16),
//               Language.English,
//               "Learning OOP in C#",
//               "OOP, C#",
//               "Object-Oriented-Csharp",
//                "<p>Object Oriented C#</p>"));

//         });
//         _context.WithMock<ICodeFormatter>();
//         _context.WithMock<IImageProcessor>();
//         //_context.WithMock<IImageContext>();
//         //_context.WithType<DraftSaveCommand>();
//         _context.WithType<DraftValidator>();
//      }

//      private readonly ServiceTestContext<AdminServices> _context;

//      [Fact]
//      public void GetDrafts_get_all_of_them()
//      {
//         using (var svc = _context.GetService())
//         {
//            var drafts = svc.GetDrafts();

//            drafts.Should().HaveCount(3);

//            drafts
//                .First()
//                .Should()
//                .BeEquivalentTo(new
//                {
//                   Id = 1,
//                   Title = "Javascript FP",
//                   Published = true
//                });

//            drafts
//                .ElementAt(1)
//                .Should()
//                .BeEquivalentTo(new
//                {
//                   Id = 2,
//                   Title = "Object Oriented C#",
//                   Published = true
//                });

//            drafts
//                .ElementAt(2)
//                .Should()
//                .BeEquivalentTo(new
//                {
//                   Id = 3,
//                   Title = "جاوا و ویندوز",
//                   Published = false
//                });
//         }
//      }

//      [Fact]
//      public void Get()
//      {
//         using (var svc = _context.GetService())
//         {
//            var post = svc.Get(1);
//            post.Content.Should().Be("<p>JS Functional Programming</p>");
//            post.Id.Should().Be(1);
//            post.Language.Should().Be(Language.English);
//            post.Summary.Should().Be("Learning FP in Javascript");
//            post.Tags.Should().Be("JS, FP, Node.js");
//            post.Title.Should().Be("Javascript FP");
//         }
//      }





//      [Fact]
//      public void GetView()
//      {
//         var date = Post.ToLongPersianDate(DateTime.Now);
//         using (var svc = _context.GetService())
//         {
//            svc.GetView(3)
//               .Should()
//               .BeEquivalentTo(new
//               {
//                  Title = "جاوا و ویندوز",
//                  Date = date,
//                  Content = "<p>جاوا و ویندوز</p>",
//                  Tags = new[] { "Java" },
//                  Language = Language.Farsi
//               });
//         }
//      }
