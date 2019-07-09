using Blog.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
{
    public class DraftTests
    {
        private IEnumerable<Image> RenderImages(string html)
        {
            var draft = new Draft();
            draft.Info = new PostInfo { Title = "the post" };
            draft.Content = html;
            return draft.RenderImages();
        }

        #region Publishing

        private string Publish(string html)
        {
            var draft = new Draft();
            draft.Info = new PostInfo { Title = "the post" };
            draft.Content = html;
            draft.RenderImages();
            return draft.Publish().Content;
        }

        [Fact]
        public void Return_the_same_for_not_known_elements() =>
            Assert.Equal(
                "<span>TEXT</span><h3>H3</h3><h4>H4</h4>",
                Publish("<span contenteditable=\"true\">TEXT</span><h3>H3</h3><h4>H4</h4>"));

        [Fact]
        public void Delete_empty_paragraphs() =>
            Assert.Equal(
                "<p>1</p><p>2</p>",
                Publish("<p>1</p><p> </p><p>2</p>"));

        [Fact]
        public void Remove_empty_spaces() =>
            Assert.Equal(
                "<p>Hello</p>",
                Publish("<p> Hello </p>"));

        [Fact]
        public void Wrap_codes() =>
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                Publish("<pre class=\"code\"><b>CODE</b></pre>"));

        [Fact]
        public void Wrap_terminals() =>
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                Publish("<pre class=\"terminal\"><b>CMD</b></pre>"));

        [Fact]
        public void Wrap_notes() =>
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                Publish("<div class=\"note\"><b>NOTE</b></div>"));

        [Fact]
        public void Wrap_warnings() =>
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                Publish("<div class=\"warning\"><b>WARN</b></div>"));

        [Fact]
        public void Unordered_lists() =>
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                Publish("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"));

        [Fact]
        public void Ordered_lists() =>
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                Publish("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"));

        [Fact]
        public void Dont_touch_insiders() =>
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                Publish("<p contenteditable=\"true\"><strong>Hello</strong>World</p>"));

        [Fact]
        public void Set_img_src_to_file() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"><figcaption>CAP</figcaption></figure>",
                Publish("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption>CAP</figcaption></figure>"));

        [Fact]
        public void Figures_without_captions() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"));

        [Fact]
        public void Figures_with_empty_captions() =>
            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.jpeg\"></figure>",
                Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>"));

        #endregion

        [Fact]
        public void Throw_when_filename_is_not_available() =>
            Assert.Throws<InvalidOperationException>(() =>
                RenderImages("<figure><img src=\"data:image/jpeg;base64,DATA\"></figure>")
            );

        [Fact]
        public void Multiple_images_with_the_same_name()
        {
            var draft = new Draft();
            draft.Info = new PostInfo { Title = "the post" };
            draft.Content = "<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure><figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>";
            var images = draft.RenderImages();
            var publish = draft.Publish();

            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure><figure><img src=\"\\images\\posts\\the-post\\pic-1.png\"></figure>",
                publish.Content);
            Assert.Equal("pic.png", images.First().Filename);
            Assert.Equal("pic-1.png", images.ElementAt(1).Filename);
        }

        [Fact]
        public void Update_content_to_file_paths_instead_of_data_urls()
        {
            var draft = new Draft();
            draft.Info = new PostInfo { Title = "the post" };
            draft.Content = "<figure><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"></figure>";

            draft.RenderImages();

            Assert.Equal(
                "<figure><img src=\"\\images\\posts\\the-post\\pic.png\"></figure>",
                draft.Content);
        }
    }
}
