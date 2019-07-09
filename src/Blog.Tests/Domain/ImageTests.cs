using Blog.Domain;
using HtmlAgilityPack;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
    public class ImageTests
    {
        private Image CreateImage(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return Image.Create(doc.DocumentNode.FirstChild, "url");
        }

        [Fact]
        public void Throw_for_invalid_base64() =>
          Assert.Throws<InvalidOperationException>(() =>
                CreateImage("<img src=\"data:image/jpeg;base64,DATA_=\">"));

        [Fact]
        public void Create_image_fullname_and_bytes()
        {
            var img = CreateImage("<img src=\"data:image/jpeg;base64,DATA\">");
            Assert.Equal(new byte[] { 12, 4, 192 }, img.Data);
            Assert.Equal("url", img.Fullname);
        }
    }
}
