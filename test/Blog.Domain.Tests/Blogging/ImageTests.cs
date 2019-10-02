using Blog.Domain.Blogging;
using FluentAssertions;
using HtmlAgilityPack;
using System;
using Xunit;

namespace Blog.Domain.Tests.Blogging
{
   public class ImageTests
   {
      private Image CreateImage(string html)
      {
         var doc = new HtmlDocument();
         doc.LoadHtml(html);
         return Image.Create(doc.DocumentNode.FirstChild, "file", "the-post");
      }

      [Fact]
      public void Throw_for_invalid_base64()
      {
         Action act = () => CreateImage("<img src=\"data:image/jpeg;base64,DATA_=\">");
         act.Should().Throw<InvalidOperationException>();
      }

      [Fact]
      public void Create_image_data()
      {
         var img = CreateImage("<img src=\"data:image/jpeg;base64,DATA\">");
         img.Data.Should().Contain(new byte[] { 12, 4, 192 });
      }

      [Fact]
      public void Create_relative_path_of_image()
      {
         var img = CreateImage("<img src=\"data:image/jpeg;base64,DATA\">");
         img.RelativePath.Should().BePath("/images/posts/the-post/file");
      }
   }
}
