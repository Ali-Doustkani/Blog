using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Services.Administrator;
using Blog.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services
{
   [Trait("Category", "Integration")]
   public class AdminiServiceTests
   {
      public AdminiServiceTests()
      {
         _context = new ServiceTestContext<AdminServices>();
         _context.Seed(db =>
         {
            db.Infos.Add(new PostInfo("Javascript FP")
            {
               Id = 1,
               Language = Language.English,
               Summary = "Learning FP in Javascript",
               Tags = "JS, FP, Node.js"
            });
            db.Infos.Add(new PostInfo("Object Oriented C#")
            {
               Id = 2,
               Language = Language.English,
               Summary = "Learning OOP in C#",
               Tags = "OOP, C#",
            });
            db.Infos.Add(new PostInfo("جاوا و ویندوز")
            {
               Id = 3,
               Language = Language.Farsi,
               Summary = "استفاده از جاوا در ویندوز",
               Tags = "Java",
               EnglishUrl = "java-windows"
            });
            db.Drafts.Add(new Draft
            {
               Id = 1,
               Content = "<p>JS Functional Programming</p>"
            });
            db.Drafts.Add(new Draft
            {
               Id = 2,
               Content = "<p>Object Oriented C#</p>"
            });
            db.Drafts.Add(new Draft
            {
               Id = 3,
               Content = "<p>جاوا و ویندوز</p>"
            });
            db.Posts.Add(new Post(new DateTime(2019, 1, 1))
            {
               Id = 1,
               Title = "Javascript FP",
               Language = Language.English,
               Summary = "Learning FP in Javascript",
               Tags = "JS, FP, Node.js",
               Url = "Javascript-FP",
               PostContent = new PostContent { Id = 1, Content = "<p>JS Functional Programming</p>" }
            });
            db.Posts.Add(new Post(new DateTime(2019, 7, 16))
            {
               Id = 2,
               Title = "Object Oriented C#",
               Language = Language.English,
               Summary = "Learning OOP in C#",
               Tags = "OOP, C#",
               Url = "Object-Oriented-Csharp",
               PostContent = new PostContent { Id = 2, Content = "<p>Object Oriented C#</p>" }
            });

         });
         _context.WithMock<ICodeFormatter>();
         _context.WithMock<IImageProcessor>();
         _context.WithMock<IImageContext>();
         _context.WithType<DraftSaveCommand>();
         _context.WithType<DraftValidator>();
      }

      private readonly ServiceTestContext<AdminServices> _context;

      [Fact]
      public void New_post_date_is_set_to_today()
      {
         using (var svc = _context.GetService())
         {
            svc.Create()
               .PublishDate
               .Should()
               .HaveDay(DateTime.Now.Day)
               .And
               .HaveMonth(DateTime.Now.Month)
               .And
               .HaveYear(DateTime.Now.Year);
         }
      }

      [Fact]
      public void GetDrafts_get_all_of_them()
      {
         using (var svc = _context.GetService())
         {
            var drafts = svc.GetDrafts();

            drafts.Should().HaveCount(3);

            drafts
                .First()
                .Should()
                .BeEquivalentTo(new
                {
                   Id = 1,
                   Title = "Javascript FP",
                   Published = true
                });

            drafts
                .ElementAt(1)
                .Should()
                .BeEquivalentTo(new
                {
                   Id = 2,
                   Title = "Object Oriented C#",
                   Published = true
                });

            drafts
                .ElementAt(2)
                .Should()
                .BeEquivalentTo(new
                {
                   Id = 3,
                   Title = "جاوا و ویندوز",
                   Published = false
                });
         }
      }

      [Fact]
      public void Get()
      {
         using (var svc = _context.GetService())
         {
            var post = svc.Get(1);
            post.Content.Should().Be("<p>JS Functional Programming</p>");
            post.Id.Should().Be(1);
            post.Language.Should().Be(Language.English);
            post.PublishDate.Should().Be(new DateTime(2019, 1, 1));
            post.Summary.Should().Be("Learning FP in Javascript");
            post.Tags.Should().Be("JS, FP, Node.js");
            post.Title.Should().Be("Javascript FP");
         }
      }

      [Fact]
      public void Add_new_drafts()
      {
         var entry = new DraftEntry
         {
            Content = "<h1>Content</h1>",
            Language = Language.English,
            Summary = "Summary",
            Tags = "tagA, tagB",
            Title = "Title"
         };

         using (var svc = _context.GetService())
            svc.Save(entry);

         using (var svc = _context.GetService())
         {
            entry.Id = 4;
            svc.Get(4)
               .Should()
               .BeEquivalentTo(entry);
         }
      }

      [Fact]
      public void Update_existing_drafts()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Id = 1,
               Content = "<h1>FP</h1>",
               Title = "Javascript FP",
               Summary = "Summary",
               Tags = "Tags",
               Language = Language.English
            });
         }

         using (var svc = _context.GetService())
         {
            svc.Get(1)
               .Should()
               .BeEquivalentTo(new
               {
                  Id = 1,
                  Content = "<h1>FP</h1>",
                  Title = "Javascript FP",
                  Summary = "Summary",
                  Tags = "Tags",
                  Language = Language.English
               });
         }
      }

      [Fact]
      public void Update_draft_title()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Id = 12,
               Content = "<figure><img src=\"/images/posts/learn-js/a.png\"></figure>".Local(),
               Title = "learn js",
               Summary = "Summary",
               Tags = "Tags",
               Language = Language.English,
               PublishDate = new DateTime(2018, 8, 8)
            });
         }

         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Id = 12,
               Content = "<figure><img src=\"/images/posts/Learn-js/a.png\"></figure>".Local(),
               Title = "learn c",
               Summary = "Summary",
               Tags = "Tags",
               Language = Language.English,
               PublishDate = new DateTime(2018, 8, 8)
            });
         }

         using (var svc = _context.GetService())
         {
            svc.Get(12)
               .Should()
               .BeEquivalentTo(new
               {
                  Title = "learn c",
                  Content = "<figure><img src=\"/images/posts/learn-c/a.png\"></figure>".Local()
               });
         }
      }

      [Fact]
      public void Create_new_post_of_a_draft()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Content = "<h1>Content</h1>",
               Language = Language.English,
               PublishDate = new DateTime(2019, 1, 1),
               Publish = true,
               Summary = "Summary",
               Tags = "Tags",
               Title = "Title"
            });
         }

         using (var svc = _context.GetService())
         {
            svc.Get(4)
               .Should()
               .BeEquivalentTo(new
               {
                  Content = "<h1>Content</h1>"
               });
         }
      }

      [Fact]
      public void Update_old_post_of_a_draft()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Id = 1,
               Content = "<p>New Content</p>",
               Publish = true,
               Title = "new title",
               Summary = "new summary",
               Tags = "new tag",
               Language = Language.English,
               PublishDate = new DateTime(2017, 7, 7)
            });
         }

         using (var db = _context.GetDatabase())
         {
            db.Posts
               .Include(x => x.PostContent)
               .Single(x => x.Id == 1)
               .Should()
               .BeEquivalentTo(new
               {
                  Url = "new-title",
                  PostContent = new { Content = "<p>New Content</p>" },
               });
         }
      }

      [Fact]
      public void Delete_post_when_draft_publish_is_not_checked()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Id = 1,
               Title = "New Title",
               Content = "<p>New Content</p>",
               Summary = "SUMMARY",
               Tags = "Tags",
               Publish = false,
               Language = Language.English
            });

         }

         using (var db = _context.GetDatabase())
         {
            db.Posts
               .SingleOrDefault(x => x.Id == 1)
               .Should()
               .BeNull();
         }
      }

      [Fact]
      public void Save_images()
      {
         using (var svc = _context.GetService())
         {
            svc.Save(new DraftEntry
            {
               Content = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>",
               Title = "the post",
               Summary = "SUMMARY",
               Language = Language.English,
               Tags = "tags"
            });

            _context.GetMock<IImageContext>()
               .Verify(x => x.SaveChanges(null, "the-post", It.IsAny<IEnumerable<Image>>()));
         }
      }

      [Fact]
      public void Delete_draft()
      {
         using (var svc = _context.GetService())
         {
            svc.Delete(3);
         }

         using (var db = _context.GetDatabase())
         {
            db.Infos
                .Should()
                .HaveCount(2);

            db.Drafts
                .Should()
                .HaveCount(2);

            db.Posts
                .Should()
                .HaveCount(2);
         }
      }

      [Fact]
      public void Delete_post()
      {
         using (var svc = _context.GetService())
         {
            svc.Delete(1);
         }

         using (var db = _context.GetDatabase())
         {
            db.Infos
                .Should()
                .HaveCount(2);

            db.Drafts
              .Should()
              .HaveCount(2);

            db.Posts
              .Should()
              .HaveCount(1);
         }
      }

      [Fact]
      public void GetView()
      {
         var date = Post.ToLongPersianDate(DateTime.Now);
         using (var svc = _context.GetService())
         {
            svc.GetView(3)
               .Should()
               .BeEquivalentTo(new
               {
                  Title = "جاوا و ویندوز",
                  Date = date,
                  Content = "<p>جاوا و ویندوز</p>",
                  Tags = new[] { "Java" },
                  Language = Language.Farsi
               });
         }
      }

      [Fact]
      public void If_code_formatting_failed_just_save_the_draft()
      {
         _context.GetMock<ICodeFormatter>()
            .Setup(x => x.Format(It.IsAny<string>(), It.IsAny<string>()))
            .Callback(() => throw new ServiceDependencyException("Error Happened", null));

         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DraftEntry
            {
               Content = string.Join(Environment.NewLine, "<pre class=\"code\">", "js", "content</pre>"),
               Language = Language.English,
               Publish = true,
               Summary = "summary",
               Tags = "tags",
               Title = "title"
            });

            result
               .Should()
               .BeEquivalentTo(new
               {
                  Failed = true,
                  Url = (string)null,
               });

            result
               .Problems
               .Should()
               .ContainEquivalentOf(new { Property = "", Message = "Draft saved but couldn't publish. Error Happened." });
         }
      }

      [Fact]
      public void Save_the_draft_alone_if_saving_post_is_not_possible()
      {
         _context.GetMock<ICodeFormatter>()
            .SetupSequence(x => x.Format(It.IsAny<string>(), It.IsAny<string>()))
            .Throws(new ServiceDependencyException("Error happened", null))
            .Returns("CODE");

         var toAdd = new DraftEntry
         {
            Language = Language.English,
            PublishDate = new DateTime(2019, 7, 24),
            Title = "title",
            Summary = "summary",
            Tags = "tags",
            Publish = true,
            Content = "<pre class=\"code\">js\r\nCODE</pre>"
         };

         using (var svc = _context.GetService())
            toAdd.Id = svc.Save(toAdd).Id;

         using (var svc = _context.GetService())
            svc.Save(toAdd);

         using (var svc = _context.GetService())
         {
            svc.Get(4)
               .Should()
               .BeEquivalentTo(new
               {
                  Language = Language.English,
                  PublishDate = new DateTime(2019, 7, 24),
                  Title = "title",
                  Summary = "summary",
                  Tags = "tags",
                  Content = "<pre class=\"code\">js\r\nCODE</pre>"
               });
         }
      }
   }
}
