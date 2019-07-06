using Blog.Domain;
using Xunit;

namespace Blog.Tests.Model
{
    public class ContentGeneratorTests
    {
        [Fact]
        public void Return_The_Same_For_Not_Formulated_Types()
        {
            Assert.Equal(
                "<span>TEXT</span>",
                Article.Decorate("<span contenteditable=\"true\">TEXT</span>"));
        }

        [Fact]
        public void Ignore_Empty_Paragraphs()
        {
            Assert.Equal(
                "<p>1</p><p>2</p>",
                Article.Decorate("<p>1</p><p> </p><p>2</p>"));
        }

        [Fact]
        public void Ignore_Whitespaces()
        {
            Assert.Equal(
                "<p>Hello</p>",
                Article.Decorate("<p> Hello </p>"));
        }

        [Fact]
        public void Direct_Elements()
        {
            Assert.Equal(
                "<p>content</p><h3>H3</h3><h4>H4</h4>",
                Article.Decorate("<p contenteditable=\"true\">content</p><h3>H3</h3><h4>H4</h4>"));
        }

        [Fact]
        public void Code()
        {
            Assert.Equal(
                "<div class=\"code\"><pre><b>CODE</b></pre></div>",
                Article.Decorate("<pre class=\"code\"><b>CODE</b></pre>"));
        }

        [Fact]
        public void Terminal()
        {
            Assert.Equal(
                "<div class=\"cmd\"><pre><b>CMD</b></pre></div>",
                Article.Decorate("<pre class=\"terminal\"><b>CMD</b></pre>"));
        }

        [Fact]
        public void Note()
        {
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>",
                Article.Decorate("<div class=\"note\"><b>NOTE</b></div>"));
        }

        [Fact]
        public void Warning()
        {
            Assert.Equal(
                "<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>",
                Article.Decorate("<div class=\"warning\"><b>WARN</b></div>"));
        }

        [Fact]
        public void UnorderedList()
        {
            Assert.Equal(
                "<ul><li>I1</li><li><b>I2</b></li></ul>",
                Article.Decorate("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"));
        }

        [Fact]
        public void OrderedList()
        {
            Assert.Equal(
                "<ol><li>I1</li><li><b>I2</b></li></ol>",
                Article.Decorate("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"));
        }

        [Fact]
        public void Tags_Inside()
        {
            Assert.Equal(
                "<p><strong>Hello</strong>World</p>",
                Article.Decorate("<p contenteditable=\"true\"><strong>Hello</strong>World</p>"));
        }
    }
}
