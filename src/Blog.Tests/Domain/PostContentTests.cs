using Blog.Domain;
using System;
using System.Linq;
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
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
            post.Render("the-post");
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Return_Image_Fullname_And_Data()
        {
            var post = new PostContent
            {
                MarkedContent = "<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption>CAP</figcaption></figure>"
            };

            var images = post.Render("the-post");

            Assert.Single(images);
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"><figcaption>CAP</figcaption></figure>",
                post.DisplayContent);
            Assert.Equal(new byte[] { 12, 4, 192 }, images.First().Data);
            Assert.Equal($"wwwroot\\images\\posts\\the-post\\pic.png", images.First().Fullname);
        }

        [Fact]
        public void Render_Figures_Without_Captions()
        {
            var post = new PostContent
            {
                MarkedContent = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"
            };

            post.Render("the-post");

            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                post.DisplayContent);
        }

        [Fact]
        public void Render_Figures_With_Empty_Captions()
        {
            var post = new PostContent
            {
                MarkedContent = "<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>"
            };

            post.Render("the-post");

            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                post.DisplayContent);
        }

        [Fact]
        public void When_Filename_Is_Not_Available_Use_Random_Name()
        {
            var post = new PostContent
            {
                MarkedContent = "<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>"
            };

            var images = post.Render("the-post");

            Assert.Equal(
                $"<figure><img src=\"\\images\\posts\\the-post\\{images.First().Filename}\"></figure>",
                post.DisplayContent);
        }

        [Fact]
        public void Throw_For_Invalid_Base64()
        {
            var post = new PostContent
            {
                MarkedContent = "<figure><img src=\"data:image/jpeg;base64,DATA_=\"></figure>"
            };

            Assert.Throws<InvalidOperationException>(() => post.Render("the-post"));
        }
    }
}
