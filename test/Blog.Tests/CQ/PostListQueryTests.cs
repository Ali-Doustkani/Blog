using Blog.CQ.PostListQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.CQ
{
   public class PostListQueryTests
   {
      public PostListQueryTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         _handler = new Handler(_context.GetDb());
      }

      private readonly TestContext _context;
      private IRequestHandler<PostListQuery, IEnumerable<PostItem>> _handler;

      [Fact]
      public async Task Get_post_items()
      {
         using (var db = _context.GetDb())
         {
            var draft0 = new Draft(0, "C#", null, Language.English, "learn c#", ".net", "<p>text</p>");
            db.Drafts.Add(draft0);

            var draft1 = new Draft(0, "JS", null, Language.English, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft1);
            var post1 = new Post(2, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post1);

            var draft2 = new Draft(0, "React", null, Language.Farsi, "learn react", "js", "<p>text</p>");
            db.Drafts.Add(draft2);
            var post2 = new Post(3, "React", new DateTime(2011, 1, 1), Language.Farsi, "learn react", "js", "react", "<p>text</p>");
            db.Posts.Add(post2);
            db.SaveChanges();
         }

         // query english posts

         var result = await _handler.Handle(new PostListQuery { Language = Language.English }, default);

         result.Should().HaveCount(1);
         result.First().Should().BeEquivalentTo(new PostItem
         {
            Title = "JS",
            Summary = "learn js",
            Tags = new[] { "js", "es" },
            Date = "Jan 2010",
            Url = "js"
         });

         // query farsi posts

         result = await _handler.Handle(new PostListQuery { Language = Language.Farsi }, default);

         result.Should().HaveCount(1);
         result.First().Should().BeEquivalentTo(new PostItem
         {
            Title = "React",
            Summary = "learn react",
            Tags = new[] { "js" },
            Date = "دی 1389",
            Url = "react"
         });
      }
   }
}
