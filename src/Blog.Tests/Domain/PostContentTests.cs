using Blog.Domain;
using Blog.Utils;
using HtmlAgilityPack;
using Moq;
using Xunit;

namespace Blog.Tests.Domain
{
    public class PostContentTests
    {
        [Fact]
        public void Return_The_Same_For_Not_Formulated_Types()
        {
            var post = new PostContent
            {
                MarkedContent = "<span contenteditable=\"true\">TEXT</span>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<span>TEXT</span>",
                post.DisplayContent);
        }

        [Fact]
        public void Ignore_Empty_Paragraphs()
        {
            var post = new PostContent
            {
                MarkedContent = "<p>1</p><p> </p><p>2</p>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<p>1</p><p>2</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Ignore_Whitespaces()
        {
            var post = new PostContent
            {
                MarkedContent = "<p> Hello </p>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<p>Hello</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Direct_Elements()
        {
            var post = new PostContent
            {
                MarkedContent = "<p contenteditable=\"true\">content</p><h3>H3</h3><h4>H4</h4>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<p>content</p><h3>H3</h3><h4>H4</h4>",
                post.DisplayContent);
        }

        [Fact]
        public void Code()
        {
            var post = new PostContent
            {
                MarkedContent = "<pre class=\"code\"><b>CODE</b></pre>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Terminal()
        {
            var post = new PostContent
            {
                MarkedContent = "<pre class=\"terminal\"><b>CMD</b></pre>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Note()
        {
            var post = new PostContent
            {
                MarkedContent = "<div class=\"note\"><b>NOTE</b></div>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Warning()
        {
            var post = new PostContent
            {
                MarkedContent = "<div class=\"warning\"><b>WARN</b></div>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                post.DisplayContent);
        }

        [Fact]
        public void UnorderedList()
        {
            var post = new PostContent
            {
                MarkedContent = "<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                post.DisplayContent);
        }

        [Fact]
        public void OrderedList()
        {
            var post = new PostContent
            {
                MarkedContent = "<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                post.DisplayContent);
        }

        [Fact]
        public void Tags_Inside()
        {
            var post = new PostContent
            {
                MarkedContent = "<p contenteditable=\"true\"><strong>Hello</strong>World</p>"
            };
            post.Render(null, string.Empty);
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Images()
        {
            var imgSvc = new Mock<IImageSaver>();
            imgSvc.Setup(x => x.Save(It.IsAny<string>(), It.IsAny<HtmlNode>())).Returns("PATH");
            var post = new PostContent
            {
                MarkedContent = "<figure><button>Remove</button><img src=DATA/><figcaption>CAP</figcaption></figure>"
            };

            post.Render(imgSvc.Object, "sth");

            Assert.Equal(
                "<figure><img src=\"PATH\"><figcaption>CAP</figcaption></figure>",
                post.DisplayContent);
        }
    }
}
