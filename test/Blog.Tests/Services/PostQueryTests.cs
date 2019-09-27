using AutoMapper;
using Blog.Services.PostQuery;
using Blog.Domain;
using Blog.Domain.Blogging;
using FluentAssertions;
using MediatR;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.Services
{
   public class PostQueryTests
   {
      public PostQueryTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(_context.GetDb(), mapper);
      }

      private readonly TestContext _context;
      private IRequestHandler<PostQuery, PostViewModel> _handler;

      [Fact]
      public async Task Get_post()
      {
         using (var db = _context.GetDb())
         {
            var draft1 = new Draft(0, "JS", null, Language.Farsi, "learn js", "js, es", "<p>text</p>");
            db.Drafts.Add(draft1);
            var post1 = new Post(1, "JS", new DateTime(2010, 1, 1), Language.Farsi, "learn js", "js, es", "js", "<p>text</p>");
            db.Posts.Add(post1);

            var draft2 = new Draft(0, "C#", null, Language.English, "learn c#", ".net", "<p>text</p>");
            db.Drafts.Add(draft2);
            var post2 = new Post(2, "C#", new DateTime(2011, 1, 1), Language.English, "learn c#", ".net", "c-sharp", "<p>text</p>");
            db.Posts.Add(post2);

            db.SaveChanges();
         }

         // query farsi post

         var result = await _handler.Handle(new PostQuery { PostUrl = "js" }, default);

         result.Should().BeEquivalentTo(new PostViewModel
         {
            Id = 1,
            Title = "JS",
            Language = Language.Farsi,
            Tags = new[] { "js", "es" },
            Content = "<p>text</p>",
            Date = "جمعه، 11 دی 1388"
         });

         // query english post

         result = await _handler.Handle(new PostQuery { PostUrl = "c-sharp" }, default);

         result.Should().BeEquivalentTo(new PostViewModel
         {
            Id = 2,
            Title = "C#",
            Language = Language.English,
            Tags = new[] { ".net" },
            Content = "<p>text</p>",
            Date = "Saturday, January 1, 2011"
         });
      }
   }
}
