using Blog.Domain;
using Blog.Utils;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Utils
{
   public class ImageContextTests
   {
      public ImageContextTests()
      {
         _log = new List<string>();
         _fs = new Mock<IFileSystem>();

         _fs.Setup(x => x
         .WriteAllBytes(It.IsAny<string>(), It.IsAny<byte[]>()))
             .Callback((string path, byte[] data) =>
                 _log.Add(string.Join(" ", "write-file", path.Local(), string.Join(",", data))));

         _fs.Setup(x => x
         .CreateDirectory(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "create-dir", path.Local())));

         _fs.Setup(x => x
         .DeleteFile(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "del-file", path.Local())));

         _fs.Setup(x => x
         .DeleteDirectory(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "del-dir", path.Local())));

         _fs.Setup(x => x
         .RenameDirectory(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string oldDir, string newDir) =>
            _log.Add(string.Join(" ", "rename-dir", oldDir.Local(), newDir.Local())));

         _ctx = new ImageContext(_fs.Object);
      }

      private readonly ImageContext _ctx;
      private readonly Mock<IFileSystem> _fs;
      private readonly List<string> _log;

      [Fact]
      public void Write_image_files()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
            .Returns(new[] { "a.png", "b.png" });

         var images = new[] {
                new Image("a.png", "the-post", new byte[] { 1, 2, 3 }),
                new Image("b.png", "the-post", new byte[] {4,5})
            };

         _ctx.SaveChanges(null, "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
                "create-dir wwwroot/images/posts/the-post",
                "write-file wwwroot/images/posts/the-post/a.png 1,2,3",
                "write-file wwwroot/images/posts/the-post/b.png 4,5"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Ignore_written_files()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
             .Returns(new[] { "a.png", "b.png" });

         var images = new[]
         {
                new Image("a.png", "the-post"),
                new Image("b.png", "the-post", new byte[] { 1, 2 })
            };

         _ctx.SaveChanges(string.Empty, "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
                "create-dir wwwroot/images/posts/the-post",
                "write-file wwwroot/images/posts/the-post/b.png 1,2"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Delete_orphan_files()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
             .Returns(new[]
             {
                "wwwroot/images/posts/the-post/a.png",
                "wwwroot/images/posts/the-post/b.png",
                "wwwroot/images/posts/the-post/c.png"
             });

         var images = new[]
         {
                new Image("b.png", "the-post"),
                new Image("c.png", "the-post", new byte[]{1,2})
            };

         _ctx.SaveChanges("the-post", "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
                "create-dir wwwroot/images/posts/the-post",
                "write-file wwwroot/images/posts/the-post/c.png 1,2",
                "del-file wwwroot/images/posts/the-post/a.png"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Delete_empty_directory_at_the_end()
      {
         _fs.SetupSequence(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
             .Returns(new[]
             {
                "wwwroot/images/posts/the-post/a.png",
                "wwwroot/images/posts/the-post/b.png"
             }).Returns(new string[] { });

         var images = Enumerable.Empty<Image>();

         _ctx.SaveChanges("the-post", "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
                "del-file wwwroot/images/posts/the-post/a.png",
                "del-file wwwroot/images/posts/the-post/b.png",
                "del-dir wwwroot/images/posts/the-post"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Rename_post_directory_name()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/new-title".Local()))
            .Returns(new string[] { "a.png" });
         var images = new List<Image>();
         images.Add(new Image("a.png", "new-title"));

         _ctx.SaveChanges("the-post", "new-title", images);

         _log.Should().BeEquivalentTo(new[]
         {
            "rename-dir wwwroot/images/posts/the-post wwwroot/images/posts/new-title",
            "create-dir wwwroot/images/posts/new-title"
         },
         cfg => cfg.WithStrictOrdering());
      }
   }
}
