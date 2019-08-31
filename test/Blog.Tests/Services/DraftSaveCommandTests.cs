using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Services.DraftSaveCommand;
using Blog.Infrastructure;
using FluentAssertions;
using MediatR;
using Moq;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.CQ
{
   public class DraftSaveCommandTests
   {
      public DraftSaveCommandTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         var context = _context.GetDb();
         _fs = new MockFileSystem();
         _imageContext = new ImageContext(_fs);
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _dateProvider = new Mock<IDateProvider>();
         _dateProvider.Setup(x => x.Now).Returns(DateTime.Now);
         _imageProcessor = new Mock<IImageProcessor>();
         _htmlProcessor = new HtmlProcessor(Mock.Of<ICodeFormatter>(), _imageProcessor.Object);
         _handler = new Handler(context, _imageContext, mapper, _dateProvider.Object, _htmlProcessor);
      }

      private readonly IRequestHandler<DraftSaveCommand, Result> _handler;
      private readonly TestContext _context;
      private readonly MockFileSystem _fs;
      private readonly ImageContext _imageContext;
      private readonly Mock<IDateProvider> _dateProvider;
      private readonly Mock<IImageProcessor> _imageProcessor;
      private readonly IHtmlProcessor _htmlProcessor;

      [Fact]
      public async Task Add_new_drafts()
      {
         var entry = new DraftSaveCommand
         {
            Title = "JS",
            Summary = "Learn JS",
            Tags = "js, es",
            Language = Language.English,
            Content = "<p>text</p>",
            EnglishUrl = "learn-js"
         };

         var result = await _handler.Handle(entry, default);

         result.Should().BeEquivalentTo(new
         {
            PostUrl = (string)null,
            Published = false
         });

         using (var ctx = _context.GetDb())
         {
            ctx.Drafts.Should().HaveCount(1);
            ctx.Drafts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "JS",
               Summary = "Learn JS",
               Language = Language.English,
               Content = "<p>text</p>",
               EnglishUrl = "learn-js"
            });
         }

      }

      [Fact]
      public async Task Update_existing_drafts()
      {
         using (var db = _context.GetDb())
         {
            var draft = new Draft(0, "JS", null, Language.English, "Learn JS", "js, es", "<p>text</p>");
            db.Drafts.Add(draft);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Id = 1,
            Title = "Functional JS",
            Summary = "Learn FP with JS",
            Tags = "js, es",
            Language = Language.English,
            Content = "<p>fp text</p>",
         }, default);

         result.Should().BeEquivalentTo(new
         {
            Published = false,
            PostUrl = (string)null
         });

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "Functional JS",
               Summary = "Learn FP with JS",
               Tags = "js, es",
               Language = Language.English,
               Content = "<p>fp text</p>"
            });
         }
      }

      [Fact]
      public async Task Save_data_urls_to_image_files()
      {
         // add a draft with images

         await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            EnglishUrl = "learn-js",
            Content = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"
         }, default);

         // make sure image files are written to disk and the src is updated accordingly

         _fs.log.Should().ContainInOrder(
            @"create-dir wwwroot/images/posts/learn-js",
            @"write-file wwwroot/images/posts/learn-js/pic.jpeg 12,4,192"
            );

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Content = "<figure><img src=\"/images/posts/learn-js/pic.jpeg\"></figure>".Local()
            });
         }

         // update the draft's url

         _fs.log.Clear();

         await _handler.Handle(new DraftSaveCommand
         {
            Id = 1,
            Title = "C#",
            EnglishUrl = "learn-c-sharp",
            Content = "<figure><img src=\"/images/posts/learn-js/pic.jpeg\"></figure>"
         }, default);

         // make sure image directory name is updated according to the new url

         _fs.log.Should().ContainInOrder(
            @"rename-dir wwwroot/images/posts/learn-js wwwroot/images/posts/learn-c-sharp"
            );

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Content = "<figure><img src=\"/images/posts/learn-c-sharp/pic.jpeg\"></figure>".Local()
            });
         }
      }

      [Fact]
      public async Task Publish_a_new_draft()
      {
         _dateProvider.Setup(x => x.Now).Returns(new DateTime(2010, 1, 1));

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<p contenteditable=\"true\">text</p>",
            Language = Language.English,
            Publish = true,
         }, default);

         result.Should().BeEquivalentTo(new
         {
            PostUrl = "js",
            Published = true
         });

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "JS",
               Summary = "learn js",
               Tags = "js, es",
               Language = Language.English,
               Content = "<p contenteditable=\"true\">text</p>"
            });

            db.Posts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "JS",
               PublishDate = new DateTime(2010, 1, 1),
               Summary = "learn js",
               Language = Language.English,
               Tags = "js, es",
               Url = "js",
               Content = "<p>text</p>"
            });
         }
      }

      [Fact]
      public async Task Publish_an_old_draft()
      {
         _dateProvider.Setup(x => x.Now).Returns(new DateTime(2010, 1, 1));

         using (var db = _context.GetDb())
         {
            var draft = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p contenteditable=\"true\">text</p>");
            db.Drafts.Add(draft);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Id = 1,
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Language = Language.English,
            Content = "<p contenteditable=\"true\">text</p>",
            Publish = true
         }, default);

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "JS",
               Summary = "learn js",
               Tags = "js, es",
               Language = Language.English,
               Content = "<p contenteditable=\"true\">text</p>"
            });

            db.Posts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "JS",
               Summary = "learn js",
               Tags = "js, es",
               Language = Language.English,
               PublishDate = new DateTime(2010, 1, 1),
               Content = "<p>text</p>"
            });
         }
      }

      [Fact]
      public async Task Update_the_published_post_when_updating_a_draft()
      {
         using (var db = _context.GetDb())
         {
            var draft = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft);
            db.Entry(draft).Property("_publishDate").CurrentValue = new DateTime(2010, 1, 1);
            var post = new Post(1, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Id = 1,
            Title = "ES7",
            Summary = "learn es",
            Tags = "js, es",
            EnglishUrl = "learn-es-7",
            Language = Language.English,
            Content = "<p>text2</p>",
            Publish = true
         }, default);

         result.Should().BeEquivalentTo(new
         {
            PostUrl = "learn-es-7",
            Published = true
         });

         using (var db = _context.GetDb())
         {
            db.Posts.Single().Should().BeEquivalentTo(new
            {
               Id = 1,
               Title = "ES7",
               Summary = "learn es",
               Tags = "js, es",
               Url = "learn-es-7",
               Language = Language.English,
               PublishDate = new DateTime(2010, 1, 1),
               Content = "<p>text2</p>",
            });
         }
      }

      [Fact]
      public async Task Unpublish_a_draft()
      {
         using (var db = _context.GetDb())
         {
            var draft = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft);
            db.Entry(draft).Property("_publishDate").CurrentValue = new DateTime(2010, 1, 1);
            var post = new Post(1, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Id = 1,
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<p>text</p>",
            Language = Language.English,
            Publish = false
         }, default);

         result.Should().BeEquivalentTo(new
         {
            PostUrl = (string)null,
            Published = false
         });

         using (var db = _context.GetDb())
         {
            db.Drafts.Should().HaveCount(1);
            db.Posts.Should().BeEmpty();
         }
      }

      [Fact]
      public async Task When_draft_content_format_is_not_correct_just_save_the_draft()
      {
         var result = await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<pre class=\"code\">var a;</pre>",
            Language = Language.English,
            Publish = true
         }, default);

         result.Published.Should().BeFalse();
         result.Errors.Should().ContainEquivalentOf(new
         {
            Message = "Language is not specified for the code block #1"
         });

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Content = "<pre class=\"code\">var a;</pre>"
            });
            db.Posts.Should().BeEmpty();
         }
      }

      [Fact]
      public async Task If_publishing_failed_save_the_draft_anyway()
      {
         _imageProcessor.Setup(x => x.Minimize(It.IsAny<string>()))
            .Throws(new ServiceDependencyException("reason", new Exception()));

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<figure><img data-filename=\"a.png\" src=\"data:image/jpeg;base64,DATA\"></figure>",
            Language = Language.English,
            Publish = true
         }, default);

         result.PostUrl.Should().BeNull();
         result.Published.Should().BeFalse();
         result.Errors.Should().ContainEquivalentOf(new
         {
            Message = "reason"
         });

         using (var db = _context.GetDb())
         {
            db.Drafts.Single().Should().BeEquivalentTo(new
            {
               Content = "<figure><img src=\"/images/posts/js/a.png\"></figure>".Local()
            });
            db.Posts.Should().BeEmpty();
         }
      }

      [Fact]
      public async Task Dont_add_drafts_with_duplicate_titles()
      {
         await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<p>text</p>",
            Language = Language.English
         }, default);

         var result = await _handler.Handle(new DraftSaveCommand
         {
            Title = "JS",
            Summary = "learn js",
            Tags = "js, es",
            Content = "<p>text</p>",
            Language = Language.English
         }, default);

         result.Errors.Should().ContainEquivalentOf(new
         {
            Message = "A draft or post with title 'JS' already exists"
         });

      }
   }
}
