using Blog.Model;
using Xunit;

namespace Blog.Tests.Model
{
    public class ArticleTests
    {
        public ArticleTests()
        {
            article = new Article(null);
        }

        readonly Article article;

        [Fact]
        public void Return_The_Same_For_Not_Formulated_Types()
        {
            var post = new Post { MarkedContent = "<span contenteditable=\"true\">TEXT</span>" };
            article.Decorate(post);
            Assert.Equal(
                "<span>TEXT</span>",
                post.DisplayContent);
        }

        [Fact]
        public void Ignore_Empty_Paragraphs()
        {
            var post = new Post { MarkedContent = "<p>1</p><p> </p><p>2</p>" };
            article.Decorate(post);
            Assert.Equal(
                "<p>1</p><p>2</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Ignore_Whitespaces()
        {
            var post = new Post { MarkedContent = "<p> Hello </p>" };
            article.Decorate(post);
            Assert.Equal(
                "<p>Hello</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Direct_Elements()
        {
            var post = new Post { MarkedContent = "<p contenteditable=\"true\">content</p><h3>H3</h3><h4>H4</h4>" };
            article.Decorate(post);
            Assert.Equal(
                "<p>content</p><h3>H3</h3><h4>H4</h4>",
                post.DisplayContent);
        }

        [Fact]
        public void Code()
        {
            var post = new Post { MarkedContent = "<pre class=\"code\"><b>CODE</b></pre>" };
            article.Decorate(post);
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Terminal()
        {
            var post = new Post { MarkedContent = "<pre class=\"terminal\"><b>CMD</b></pre>" };
            article.Decorate(post);
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Note()
        {
            var post = new Post { MarkedContent = "<div class=\"note\"><b>NOTE</b></div>" };
            article.Decorate(post);
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                post.DisplayContent);
        }

        [Fact]
        public void Warning()
        {
            var post = new Post { MarkedContent = "<div class=\"warning\"><b>WARN</b></div>" };
            article.Decorate(post);
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                post.DisplayContent);
        }

        [Fact]
        public void UnorderedList()
        {
            var post = new Post { MarkedContent = "<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>" };
            article.Decorate(post);
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                post.DisplayContent);
        }

        [Fact]
        public void OrderedList()
        {
            var post = new Post { MarkedContent = "<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>" };
            article.Decorate(post);
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                post.DisplayContent);
        }

        [Fact]
        public void Tags_Inside()
        {
            var post = new Post { MarkedContent = "<p contenteditable=\"true\"><strong>Hello</strong>World</p>" };
            article.Decorate(post);
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                post.DisplayContent);
        }

        [Fact]
        public void Images()
        {
            var post = new Post { MarkedContent = "<figure><button>Remove</button><img src=DATA/><figcaption>CAP</figcaption></figure>" };
            article.Decorate(post);
            Assert.Equal(
                "<figure><img src=\"images/posts/p_title/img1.jpg\"><figcaption>CAP</figcaption></figure>",
                post.DisplayContent);
        }
    }
}
