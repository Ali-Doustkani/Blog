using Blog.Domain;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
{
    public class PostContentTests
    {
        private string Render(string html)
        {
            var post = new Post();
            post.UrlTitle = "the-post";
            post.Content.MarkedContent = html;
            post.Content.Render();
            return post.Content.DisplayContent;
        }

        private PostContent Content(string html)
        {
            var post = new Post();
            post.UrlTitle = "the-post";
            post.Content.MarkedContent = html;
            return post.Content;
        }

        [Fact]
        public void Return_The_Same_For_Not_Formulated_Types()
        {
            Assert.Equal(
                "<span>TEXT</span>",
                Render("<span contenteditable=\"true\">TEXT</span>"));
        }

        [Fact]
        public void Ignore_Empty_Paragraphs()
        {
            Assert.Equal(
                "<p>1</p><p>2</p>",
                Render("<p>1</p><p> </p><p>2</p>"));
        }

        [Fact]
        public void Ignore_Whitespaces()
        {
            Assert.Equal(
                "<p>Hello</p>",
                Render("<p> Hello </p>"));
        }

        [Fact]
        public void Direct_Elements()
        {
            Assert.Equal(
                "<p>content</p><h3>H3</h3><h4>H4</h4>",
                Render("<p contenteditable=\"true\">content</p><h3>H3</h3><h4>H4</h4>"));
        }

        [Fact]
        public void Code()
        {
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                Render("<pre class=\"code\"><b>CODE</b></pre>"));
        }

        [Fact]
        public void Terminal()
        {
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                Render("<pre class=\"terminal\"><b>CMD</b></pre>"));
        }

        [Fact]
        public void Note()
        {
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                Render("<div class=\"note\"><b>NOTE</b></div>"));
        }

        [Fact]
        public void Warning()
        {
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                Render("<div class=\"warning\"><b>WARN</b></div>"));
        }

        [Fact]
        public void UnorderedList()
        {
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                Render("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"));
        }

        [Fact]
        public void OrderedList()
        {
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                Render("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"));
        }

        [Fact]
        public void Tags_Inside()
        {
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                Render("<p contenteditable=\"true\"><strong>Hello</strong>World</p>"));
        }

        [Fact]
        public void Return_Image_Fullname_And_Data()
        {
            var ctn = Content("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption>CAP</figcaption></figure>");
            ctn.Render();
            var images = ctn.GetImages();

            Assert.Single(images);
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"><figcaption>CAP</figcaption></figure>",
                ctn.DisplayContent);
            Assert.Equal(new byte[] { 12, 4, 192 }, images.First().Data);
            Assert.Equal("the-post\\pic.png", images.First().Fullname);
        }

        [Fact]
        public void Render_Figures_Without_Captions()
        {
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Render("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"));
        }

        [Fact]
        public void Render_Figures_With_Empty_Captions()
        {
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Render("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>"));
        }

        [Fact]
        public void Throw_When_Filename_Is_Not_Available()
        {
            Assert.Throws<InvalidOperationException>(() =>
                Content("<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>")
                .GetImages()
            );
        }

        [Fact]
        public void Throw_For_Invalid_Base64()
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                var ctn = Content("<figure><img src=\"data:image/jpeg;base64,DATA_=\"></figure>");
                ctn.GetImages();
            });
        }
    }
}
