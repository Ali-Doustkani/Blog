using Blog.CQ.DraftDeleteCommand;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Utils;
using FluentAssertions;
using MediatR;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.CQ
{
   public class DraftDeleteCommandTests
   {
      public DraftDeleteCommandTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         _fs = new MockFileSystem();
         _imageContext = new ImageContext(_fs);
         _handler = new Handler(_context.GetDb(), _imageContext);
      }

      private readonly TestContext _context;
      private readonly IRequestHandler<DraftDeleteCommand> _handler;
      private readonly MockFileSystem _fs;
      private readonly ImageContext _imageContext;

      [Fact]
      public async Task Delete_a_draft_with_its_post_and_images()
      {
         _fs.CreateDirectory("wwwroot/images/posts/js");
         _fs.WriteFile("wwwroot/images/posts/js/a.png");
         _fs.WriteFile("wwwroot/images/posts/js/b.png");
         _fs.log.Clear();

         var content = "<figure><img src=\"wwwroot/images/posts/js/a.png\"></figure>" +
            "<figure><img src=\"wwwroot/images/posts/js/b.png\"></figure>";

         using (var db = _context.GetDb())
         {
            var draft = new Draft(0, "JS", null, Language.English, "learn js", "js, es", content);
            db.Drafts.Add(draft);
            var post = new Post(1, "JS", new DateTime(2010, 1, 1), Language.English, "learn js", "js, es", "js", content);
            db.Posts.Add(post);
            db.SaveChanges();
         }

         await _handler.Handle(new DraftDeleteCommand { Id = 1 }, default);

         using (var db = _context.GetDb())
         {
            db.Drafts.Should().BeEmpty();
            db.Posts.Should().BeEmpty();
         }

         _fs.log.Should().ContainInOrder("del-dir wwwroot/images/posts/js");
      }
   }
}
