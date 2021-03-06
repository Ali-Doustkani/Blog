﻿using Blog.Domain.Blogging;
using Blog.Infrastructure;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Utils
{
   public class ImageContextTests
   {
      public ImageContextTests()
      {
         _fs = new MockFileSystem();
         _ctx = new ImageContext(_fs);
      }

      private readonly ImageContext _ctx;
      private readonly MockFileSystem _fs;

      [Fact]
      public async Task Create_dir_before_writing_images()
      {
         var images = new ImageCollection(new[] { new Image("a.png", "the-post", new byte[] { 1, 2 }) }, null, "the-post");
         await _ctx.AddOrUpdateAsync(images);

         _fs.log.Should()
            .BeEquivalentTo(new[]
            {
               "create-dir wwwroot/images/posts/the-post",
               "write-file wwwroot/images/posts/the-post/a.png 1,2"
            });
      }

      [Fact]
      public async Task Write_image_files()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         _fs.WriteFile("wwwroot/images/posts/the-post/a.png");
         _fs.WriteFile("wwwroot/images/posts/the-post/b.png");
         _fs.log.Clear();

         var images = new[]
         {
                new Image("a.png", "the-post", new byte[] { 1, 2, 3 }),
                new Image("b.png", "the-post", new byte[] {4,5})
         };
         var imageCollection = new ImageCollection(images, null, "the-post");
         await _ctx.AddOrUpdateAsync(imageCollection);

         _fs.log.Should()
             .BeEquivalentTo(new[]
             {
                "write-file wwwroot/images/posts/the-post/a.png 1,2,3",
                "write-file wwwroot/images/posts/the-post/b.png 4,5"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Ignore_written_files()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         _fs.WriteFile("wwwroot/images/posts/the-post/a.png");
         _fs.WriteFile("wwwroot/images/posts/the-post/b.png");
         _fs.log.Clear();

         var images = new[]
         {
                new Image("a.png", "the-post"),
                new Image("b.png", "the-post", new byte[] { 1, 2 })
         };
         var imageCollection = new ImageCollection(images, string.Empty, "the-post");

         await _ctx.AddOrUpdateAsync(imageCollection);

         _fs.log.Should()
             .BeEquivalentTo(new[]
             {
                "write-file wwwroot/images/posts/the-post/b.png 1,2"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Delete_orphan_files()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         _fs.WriteFile("wwwroot/images/posts/the-post/a.png");
         _fs.WriteFile("wwwroot/images/posts/the-post/b.png");
         _fs.WriteFile("wwwroot/images/posts/the-post/c.png");
         _fs.log.Clear();

         var images = new[]
         {
                new Image("b.png", "the-post"),
                new Image("c.png", "the-post", new byte[]{1,2})
         };
         var imageCollection = new ImageCollection(images, "the-post", "the-post");

         await _ctx.AddOrUpdateAsync(imageCollection);

         _fs.log.Should()
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
         _ctx.Delete("the-post");
         _fs.log.Should()
            .BeEquivalentTo(Enumerable.Empty<string>(), cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Delete_empty_directory_at_the_end()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         await _fs.WriteAllBytesAsync("wwwroot/images/posts/the-post/a.png", new byte[] { });
         await _fs.WriteAllBytesAsync("wwwroot/images/posts/the-post/b.png", new byte[] { });
         _fs.log.Clear();

         var images = new ImageCollection(Enumerable.Empty<Image>(), "the-post", "the-post");

         await _ctx.AddOrUpdateAsync(images);

         _fs.log.Should()
             .BeEquivalentTo(new[]
             {
                "del-file wwwroot/images/posts/the-post/a.png",
                "del-file wwwroot/images/posts/the-post/b.png",
                "del-dir wwwroot/images/posts/the-post"
             },
             cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Rename_post_directory_name()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         _fs.WriteFile("wwwroot/images/posts/the-post/a.png");
         _fs.log.Clear();

         var images = new List<Image>
         {
            new Image("a.png", "the-post")
         };
         var imageCollection = new ImageCollection(images, "the-post", "new-title");

         await _ctx.AddOrUpdateAsync(imageCollection);

         _fs.log.Should().BeEquivalentTo(new[]
         {
            "rename-dir wwwroot/images/posts/the-post wwwroot/images/posts/new-title",
         },
         cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Overwrite_files()
      {
         _fs.CreateDirectory("wwwroot/images/posts/the-post");
         _fs.WriteFile("wwwroot/images/posts/the-post/a.png");
         _fs.log.Clear();

         var images = new List<Image>
         {
            new Image("a.png", "the-post", new byte[] { 3, 4 })
         };
         var imageCollection = new ImageCollection(images, "the-post", "the-post");

         await _ctx.AddOrUpdateAsync(imageCollection);

         _fs.log.Should().BeEquivalentTo(new[]
         {
            "write-file wwwroot/images/posts/the-post/a.png 3,4"
         }, cfg => cfg.WithStrictOrdering());
      }

      [Fact]
      public async Task Do_not_rename_not_existing_directories()
      {
         var images = new ImageCollection(Enumerable.Empty<Image>(), "the-post", "new-title");

         await _ctx.AddOrUpdateAsync(images);

         _fs.log.Should().BeEquivalentTo(new string[] { });
      }
   }
}
