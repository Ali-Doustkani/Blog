using AutoMapper;
using Blog.CQ.DraftListQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.CQ
{
   public class DraftListQueryTests
   {
      public DraftListQueryTests()
      {
         _context = new TestContext();
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(_context.GetDb(), mapper);
      }

      private readonly TestContext _context;
      private readonly IRequestHandler<DraftListQuery, IEnumerable<DraftItem>> _handler;

      [Fact]
      public async Task GetDraftItems()
      {
         using (var db = _context.GetDb())
         {
            var draft1 = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft1);
            var draft2 = new Draft(0, "C#", "c-sharp", Language.Farsi, "learn c#", ".net", "<p>text</p>");
            db.Drafts.Add(draft2);
            var post1 = new Post(1, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post1);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DraftListQuery(), default);

         result.Should().HaveCount(2);
         result.ElementAt(0).Should().BeEquivalentTo(new
         {
            Id = 1,
            Published = true,
            Title = "JS"
         });
         result.ElementAt(1).Should().BeEquivalentTo(new
         {
            Id = 2,
            Published = false,
            Title = "C#"
         });
      }
   }
}
