using AutoMapper;
using Blog.CQ.DraftQuery;
using Blog.CQ.DraftSaveCommand;
using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using MediatR;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.CQ
{
   public class DraftQueryTests
   {
      public DraftQueryTests(ITestOutputHelper helper)
      {
         _context = new TestContext(helper);
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Blog.CQ.DraftQuery.Handler(_context.GetDb(), mapper);
      }

      private readonly TestContext _context;
      private readonly IRequestHandler<DraftQuery, DraftSaveCommand> _handler;

      [Fact]
      public async Task Get_draft()
      {
         using (var db = _context.GetDb())
         {
            var draft1 = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft1);
            var draft2 = new Draft(0, "C#", "csharp", Language.English, "learn C#", ".net", "<p>text</p>");
            db.Drafts.Add(draft2);
            var post = new Post(1, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post);
            db.SaveChanges();
         }

         (await _handler.Handle(new DraftQuery { Id = 1 }, default))
            .Should()
            .BeEquivalentTo(new DraftSaveCommand
            {
               Id = 1,
               Title = "JS",
               Summary = "learn js",
               Language = Language.English,
               Content = "<p>text</p>",
               Tags = "js, es",
               EnglishUrl = null,
               Publish = true
            });

         (await _handler.Handle(new DraftQuery { Id = 2 }, default))
            .Should()
            .BeEquivalentTo(new DraftSaveCommand
            {
               Id = 2,
               Title = "C#",
               Summary = "learn C#",
               Language = Language.English,
               Content = "<p>text</p>",
               Tags = ".net",
               EnglishUrl = "csharp",
               Publish = false
            });
      }
   }
}
