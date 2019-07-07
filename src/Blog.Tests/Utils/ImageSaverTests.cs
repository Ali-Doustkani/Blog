using Blog.Utils;
using HtmlAgilityPack;
using Moq;
using System;
using Xunit;

namespace Blog.Tests.Utils
{
    public class ImageSaverTests
    {
        public ImageSaverTests()
        {
            _fs = new Mock<IFileSystem>();
            _imageSaver = new ImageSaver(_fs.Object);
        }

        private readonly Mock<IFileSystem> _fs;
        private readonly ImageSaver _imageSaver;

        private HtmlNode Node(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.FirstChild;
        }

        [Fact]
        public void Save_To_Post_Path()
        {
            var url = _imageSaver.Save(
                "the-post",
                Node("<img src=\"data:image/jpeg;base64,DATA\" data-filename=\"fn\">"));

            Assert.Equal(@"\images\posts\the-post\fn.jpeg", url);
        }

        [Fact]
        public void When_Filename_Is_Not_Available_Use_Random_Name()
        {
            var url = _imageSaver.Save(
                "the-post",
                Node("<img src=\"data:image/png;base64,DATA\">"));

            Assert.Matches(@"^\\images\\posts\\the-post\\[a-zA-Z0-9]+\.png", url);
        }

        [Fact]
        public void Save_File_To_wwwroot()
        {
            _imageSaver.Save(
                "the-post",
                Node("<img src=\"data:image/png;base64,DATA\" data-filename=\"image\">"));
            _fs.Verify(x => x.Write(@"wwwroot\images\posts\the-post\image.png", new byte[] { 12, 4, 192 }));
        }

        [Fact]
        public void Throw_For_Invalid_Base64()
        {
            Assert.Throws<ArgumentException>(() =>
                _imageSaver.Save(
                "the-post",
                Node("<img src=\"data:image/jpeg;base64,DATA_=\">"))
            );
        }
    }
}
