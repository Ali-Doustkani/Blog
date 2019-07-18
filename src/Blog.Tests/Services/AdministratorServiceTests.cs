using AutoMapper;
using Blog.Domain;
using Blog.Services.Administrator;
using Blog.Utils;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services.Administrator
{
   [Trait("Category", "Integration")]
   public class AdministratorServiceTests
   {
      public AdministratorServiceTests()
      {
         _options = Db.CreateOptions();
         using (var seed = new BlogContext(_options))
         {
            seed.Database.EnsureCreated();
            seed.Infos.Add(new PostInfo
            {
               Id = 1,
               Language = Language.English,
               PublishDate = new DateTime(2019, 1, 1),
               Summary = "Learning FP in Javascript",
               Tags = "JS, FP, Node.js",
               Title = "Javascript FP"
            });
            seed.Infos.Add(new PostInfo
            {
               Id = 2,
               Language = Language.English,
               PublishDate = new DateTime(2019, 7, 16),
               Summary = "Learning OOP in C#",
               Tags = "OOP, C#",
               Title = "Object Oriented C#"
            });
            seed.Infos.Add(new PostInfo
            {
               Id = 3,
               Language = Language.Farsi,
               PublishDate = new DateTime(2019, 7, 16),
               Summary = "استفاده از جاوا در ویندوز",
               Tags = "Java",
               Title = "جاوا و ویندوز",
               EnglishUrl = "java-windows"
            });
            seed.Drafts.Add(new Draft
            {
               Id = 1,
               Content = "<p>JS Functional Programming</p>"
            });
            seed.Drafts.Add(new Draft
            {
               Id = 2,
               Content = "<p>Object Oriented C#</p>"
            });
            seed.Drafts.Add(new Draft
            {
               Id = 3,
               Content = "<p>جاوا و ویندوز</p>"
            });
            seed.Posts.Add(new Post
            {
               Id = 1,
               Url = "Javascript-FP",
               PostContent = new PostContent { Id = 1, Content = "<p>JS Functional Programming</p>" }
            });
            seed.Posts.Add(new Post
            {
               Id = 2,
               Url = "Object-Oriented-Csharp",
               PostContent = new PostContent { Id = 2, Content = "<p>Object Oriented C#</p>" }
            });
            seed.SaveChanges();
         }
      }

      private readonly DbContextOptions _options;
      private Mock<IImageContext> _imageContext;

      private Service Service()
      {
         var context = new BlogContext(_options);
         var config = new MapperConfiguration(cfg =>
         {
            cfg.AddProfile<PostProfile>();
            cfg.AddProfile<Blog.Services.Home.PostProfile>();
         });
         _imageContext = new Mock<IImageContext>();
         return new Service(context, config.CreateMapper(), _imageContext.Object, new DraftValidator(context));
      }

      [Fact]
      public void New_post_date_is_set_to_today()
      {
         Service()
            .Create()
            .PublishDate
            .Should()
            .HaveDay(DateTime.Now.Day)
            .And
            .HaveMonth(DateTime.Now.Month)
            .And
            .HaveYear(DateTime.Now.Year);
      }

      [Fact]
      public void GetDrafts_get_all_of_them()
      {
         var drafts = Service().GetDrafts();

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

      [Fact]
      public void Get()
      {
         var draft = Service().Get(1);

         draft.Content.Should().Be("<p>JS Functional Programming</p>");
         draft.Id.Should().Be(1);
         draft.Language.Should().Be(Language.English);
         draft.PublishDate.Should().Be(new DateTime(2019, 1, 1));
         draft.Summary.Should().Be("Learning FP in Javascript");
         draft.Tags.Should().Be("JS, FP, Node.js");
         draft.Title.Should().Be("Javascript FP");
      }

      [Fact]
      public void Dont_save_duplicate_titles()
      {
         var entry = new DraftEntry
         {
            Content = "<p>JS Content</p>",
            Title = "Javascript FP",
            Language = Language.English
         };

         var result = Service().Save(entry);

         result
            .Failed
            .Should()
            .BeTrue();

         result
            .Problems
            .Should()
            .ContainEquivalentOf(new { Message = "This title already exists in the database." });
      }

      [Fact]
      public void Add_new_drafts()
      {
         var entry = new DraftEntry
         {
            Content = "<h1>Content</h1>",
            Language = Language.English,
            PublishDate = new DateTime(2019, 1, 1),
            Summary = "Summary",
            Tags = "tagA, tagB",
            Title = "Title"
         };

         Service().Save(entry);

         entry.Id = 4;
         Service()
            .Get(4)
            .Should()
            .BeEquivalentTo(entry);
      }

      [Fact]
      public void Update_existing_drafts()
      {
         Service().Save(new DraftEntry
         {
            Id = 1,
            Content = "<h1>FP</h1>",
            Title = "Javascript FP",
            Summary = "Summary",
            Tags = "Tags",
            Language = Language.English,
            PublishDate = new DateTime(2018, 8, 8)
         });

         Service()
            .Get(1)
            .Should()
            .BeEquivalentTo(new
            {
               Id = 1,
               Content = "<h1>FP</h1>",
               Title = "Javascript FP",
               Summary = "Summary",
               Tags = "Tags",
               Language = Language.English,
               PublishDate = new DateTime(2018, 8, 8)
            });
      }

      [Fact]
      public void Update_draft_title()
      {
         Service().Save(new DraftEntry
         {
            Id = 12,
            Content = "<figure><img src=\"/images/posts/learn-js/a.png\"></figure>".Local(),
            Title = "learn js",
            Summary = "Summary",
            Tags = "Tags",
            Language = Language.English,
            PublishDate = new DateTime(2018, 8, 8)
         });

         Service().Save(new DraftEntry
         {
            Id = 12,
            Content = "<figure><img src=\"/images/posts/Learn-js/a.png\"></figure>".Local(),
            Title = "learn c",
            Summary = "Summary",
            Tags = "Tags",
            Language = Language.English,
            PublishDate = new DateTime(2018, 8, 8)
         });

         Service()
            .Get(12)
            .Should()
            .BeEquivalentTo(new
            {
               Title = "learn c",
               Content = "<figure><img src=\"/images/posts/learn-c/a.png\"></figure>".Local()
            });
      }

      [Fact]
      public void Create_new_post_of_a_draft()
      {
         Service().Save(new DraftEntry
         {
            Content = "<h1>Content</h1>",
            Language = Language.English,
            PublishDate = new DateTime(2019, 1, 1),
            Publish = true,
            Summary = "Summary",
            Tags = "Tags",
            Title = "Title"
         });

         Service()
            .Get(4)
            .Should()
            .BeEquivalentTo(new
            {
               Content = "<h1>Content</h1>"
            });
      }

      [Fact]
      public void Update_old_post_of_a_draft()
      {
         Service().Save(new DraftEntry
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

         using (var context = new BlogContext(_options))
         {
            context
                .Posts
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
         Service().Save(new DraftEntry
         {
            Id = 1,
            Title = "New Title",
            Content = "<p>New Content</p>",
            Summary = "SUMMARY",
            Tags = "Tags",
            Publish = false,
            Language = Language.English
         });

         using (var context = new BlogContext(_options))
         {
            context
                .Posts
                .SingleOrDefault(x => x.Id == 1)
                .Should()
                .BeNull();
         }
      }

      [Fact]
      public void Save_images()
      {
         Service().Save(new DraftEntry
         {
            Content = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>",
            Title = "the post",
            Summary = "SUMMARY",
            Language = Language.English,
            Tags = "tags"
         });

         _imageContext.Verify(x => x.SaveChanges(null, "the-post", It.IsAny<IEnumerable<Image>>()));
      }

      [Fact]
      public void Delete_draft()
      {
         Service().Delete(3);

         using (var ctx = new BlogContext(_options))
         {
            ctx.Infos
                .Should()
                .HaveCount(2);

            ctx.Drafts
                .Should()
                .HaveCount(2);

            ctx.Posts
                .Should()
                .HaveCount(2);
         }
      }

      [Fact]
      public void Delete_post()
      {
         Service().Delete(1);

         using (var ctx = new BlogContext(_options))
         {
            ctx.Infos
                .Should()
                .HaveCount(2);

            ctx.Drafts
              .Should()
              .HaveCount(2);

            ctx.Posts
              .Should()
              .HaveCount(1);
         }
      }

      [Fact]
      public void GetView()
      {
         Service()
            .GetView(3)
            .Should()
            .BeEquivalentTo(new
            {
               Title = "جاوا و ویندوز",
               Date = "سه شنبه، 25 تیر 1398",
               Content = "<p>جاوا و ویندوز</p>",
               Tags = new[] { "Java" },
               Language = Language.Farsi
            });
      }
   }
}
