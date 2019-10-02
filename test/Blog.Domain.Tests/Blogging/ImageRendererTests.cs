using Blog.Domain.Blogging;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Blog.Domain.Tests.Blogging
{
   public class ImageRendererTests
   {
      public ImageRendererTests()
      {
         _renderer = new ImageRenderer("the-post");
      }

      private readonly ImageRenderer _renderer;

      [Fact]
      public void Throw_when_filename_is_not_available()
      {
         Action act = () => _renderer.Render("<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>");
         act.Should().Throw<InvalidOperationException>();
      }

      [Fact]
      public void Multiple_images_with_the_same_name()
      {
         var result = _renderer.Render("<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure><figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>");

         result
            .Html
            .Should()
            .BePath("<figure><img src=\"/images/posts/the-post/pic.png\"></figure><figure><img src=\"/images/posts/the-post/pic-1.png\"></figure>");
         result
            .Images
            .First()
            .Filename
            .Should()
            .Be("pic.png");
         result
            .Images
            .ElementAt(1)
            .Filename
            .Should()
            .Be("pic-1.png");
      }

      [Fact]
      public void Update_content_to_file_paths_instead_of_data_urls() =>
         _renderer
            .Render("<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>")
            .Html
            .Should()
            .BePath("<figure><img src=\"/images/posts/the-post/pic.png\"></figure>");

      [Fact]
      public void Update_img_srcs_when_title_changes() =>
         new ImageRenderer("new-title")
            .Render("<figure><img src=\"/images/posts/the-post/pic.png\"></figure>")
            .Html
            .Should()
            .BePath("<figure><img src=\"/images/posts/new-title/pic.png\"></figure>");
   }
}
