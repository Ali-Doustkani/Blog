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

        private PostContent Create(string html)
        {
            var post = new Post();
            post.UrlTitle = "the-post";
            post.Content.MarkedContent = html;
            return post.Content;
        }

        #region Rendering HTML

        [Fact]
        public void Return_the_same_for_not_known_elements() =>
            Assert.Equal(
                "<span>TEXT</span><h3>H3</h3><h4>H4</h4>",
                Render("<span contenteditable=\"true\">TEXT</span><h3>H3</h3><h4>H4</h4>"));

        [Fact]
        public void Delete_empty_paragraphs() =>
            Assert.Equal(
                "<p>1</p><p>2</p>",
                Render("<p>1</p><p> </p><p>2</p>"));

        [Fact]
        public void Remove_empty_spaces() =>
            Assert.Equal(
                "<p>Hello</p>",
                Render("<p> Hello </p>"));

        [Fact]
        public void Wrap_codes() =>
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                Render("<pre class=\"code\"><b>CODE</b></pre>"));

        [Fact]
        public void Wrap_terminals() =>
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                Render("<pre class=\"terminal\"><b>CMD</b></pre>"));

        [Fact]
        public void Wrap_notes() =>
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                Render("<div class=\"note\"><b>NOTE</b></div>"));

        [Fact]
        public void Wrap_warnings() =>
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                Render("<div class=\"warning\"><b>WARN</b></div>"));

        [Fact]
        public void Unordered_lists() =>
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                Render("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"));

        [Fact]
        public void Ordered_lists() =>
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                Render("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"));

        [Fact]
        public void Dont_touch_insiders() =>
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                Render("<p contenteditable=\"true\"><strong>Hello</strong>World</p>"));

        [Fact]
        public void Set_img_src_to_file() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"><figcaption>CAP</figcaption></figure>",
                Render("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption>CAP</figcaption></figure>"));

        [Fact]
        public void Figures_without_captions() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Render("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"));

        [Fact]
        public void Figures_with_empty_captions() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Render("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>"));

        [Fact]
        public void Multiple_images_with_the_same_name() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure><figure><img src=\"\\images\\posts\\the-post\\pic-1.png\"></figure>",
                Render("<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure><figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>"));

        #endregion

        #region Creating Images

        [Fact]
        public void Create_image_path_and_bytes()
        {
            var images =
                Create("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>")
                .GetImages();

            Assert.Single(images);
            Assert.Equal(new byte[] { 12, 4, 192 }, images.First().Data);
            Assert.Equal("the-post\\pic.png", images.First().Fullname);
        }

        [Fact]
        public void Throw_when_filename_is_not_available() =>
            Assert.Throws<InvalidOperationException>(() =>
                Create("<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>")
                .GetImages()
            );

        [Fact]
        public void Throw_for_invalid_base64() =>
            Assert.Throws<InvalidOperationException>(() =>
            {
                var ctn = Create("<figure><img src=\"data:image/jpeg;base64,DATA_=\"></figure>");
                ctn.GetImages();
            });

        #endregion
    }
}
