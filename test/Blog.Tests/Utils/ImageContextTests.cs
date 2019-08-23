using Blog.Domain.Blogging;
using Blog.Utils;
using FluentAssertions;
using Moq;
using System.Collections.Generic;
using System.IO;
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
                 _log.Add(string.Join(" ", "write-file", path.Standard(), string.Join(",", data))));

         _fs.Setup(x => x
         .CreateDirectory(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "create-dir", path.Standard())));

         _fs.Setup(x => x
         .DeleteFile(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "del-file", path.Standard())));

         _fs.Setup(x => x
         .DeleteDirectory(It.IsAny<string>()))
             .Callback((string path) =>
             _log.Add(string.Join(" ", "del-dir", path.Standard())));

         _fs.Setup(x => x
         .RenameDirectory(It.IsAny<string>(), It.IsAny<string>()))
            .Callback((string oldDir, string newDir) =>
            _log.Add(string.Join(" ", "rename-dir", oldDir.Standard(), newDir.Standard())));

         _ctx = new ImageContext(_fs.Object);
      }

      private readonly ImageContext _ctx;
      private readonly Mock<IFileSystem> _fs;
      private readonly List<string> _log;

      [Fact]
      public void Create_dir_before_writing_images()
      {
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(false);

         var images = new[] { new Image("a.png", "the-post", new byte[] { 1, 2 }) };

         _ctx.SaveChanges(null, "the-post", images);

         _log.Should()
            .BeEquivalentTo(new[]
            {
               "create-dir wwwroot/images/posts/the-post",
               "write-file wwwroot/images/posts/the-post/a.png 1,2"
            });
      }

      [Fact]
      public void Write_image_files()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
            .Returns(new[] { "a.png", "b.png" });
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(true);

         var images = new[] {
                new Image("a.png", "the-post", new byte[] { 1, 2, 3 }),
                new Image("b.png", "the-post", new byte[] {4,5})
            };

         _ctx.SaveChanges(null, "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
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
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(true);

         var images = new[]
         {
                new Image("a.png", "the-post"),
                new Image("b.png", "the-post", new byte[] { 1, 2 })
            };

         _ctx.SaveChanges(string.Empty, "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
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
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(true);

         var images = new[]
         {
                new Image("b.png", "the-post"),
                new Image("c.png", "the-post", new byte[]{1,2})
            };

         _ctx.SaveChanges("the-post", "the-post", images);

         _log.Should()
             .BeEquivalentTo(new[]
             {
                "write-file wwwroot/images/posts/the-post/c.png 1,2",
                "del-file wwwroot/images/posts/the-post/a.png"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Delete_orphan_files_when_no_directory_exists()
      {
         _fs.Setup(x => x.GetFiles(It.IsAny<string>()))
            .Throws<DirectoryNotFoundException>();
         _fs.Setup(x => x.DirectoryExists(It.IsAny<string>()))
            .Returns(false);

         _ctx.SaveChanges("the-post", "the-post", Enumerable.Empty<Image>());

         _log.Should()
            .BeEquivalentTo(Enumerable.Empty<string>(), cfg => cfg.WithStrictOrdering());
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
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(true);

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
         var oldPath = "wwwroot/images/posts/the-post".Local();
         var newPath = "wwwroot/images/posts/new-title".Local();

         _fs.SetupSequence(x => x.DirectoryExists(oldPath))
            .Returns(true)
            .Returns(false);

         _fs.SetupSequence(x => x.GetFiles(oldPath))
            .Returns(new[] { "a.png" })
            .Throws<IOException>();

         _fs.Setup(x => x.DirectoryExists(newPath))
            .Returns(true);

         _fs.Setup(x => x.GetFiles(newPath))
            .Returns(new[] { "a.png" });

         var images = new List<Image>();
         images.Add(new Image("a.png", "the-post"));

         _ctx.SaveChanges("the-post", "new-title", images);

         _log.Should().BeEquivalentTo(new[]
         {
            "rename-dir wwwroot/images/posts/the-post wwwroot/images/posts/new-title",
         },
         cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Overwrite_files()
      {
         _fs.Setup(x => x.GetFiles("wwwroot/images/posts/the-post".Local()))
            .Returns(new[] { "a.png" });
         _fs.Setup(x => x.DirectoryExists("wwwroot/images/posts/the-post".Local()))
            .Returns(true);
         var images = new List<Image>();
         images.Add(new Image("a.png", "the-post", new byte[] { 3, 4 }));

         _ctx.SaveChanges("the-post", "the-post", images);

         _log.Should().BeEquivalentTo(new[]
         {
            "write-file wwwroot/images/posts/the-post/a.png 3,4"
         }, cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public void Do_not_rename_not_existing_directories()
      {
         var oldPath = "wwwroot/images/posts/the-post".Local();
         var newPath = "wwwroot/images/posts/new-title".Local();
         _fs.Setup(x => x.RenameDirectory(oldPath, newPath))
            .Throws<IOException>();
         _fs.Setup(x => x.DirectoryExists(oldPath))
            .Returns(false);
         var images = Enumerable.Empty<Image>();

         _ctx.SaveChanges("the-post", "new-title", images);

         _log.Should().BeEquivalentTo(new string[] { });
      }
   }
}
