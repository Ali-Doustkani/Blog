using Blog.Domain.Blogging;
using Blog.Utils;
using FluentAssertions;
using Moq;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class DraftTests
   {
      private Mock<ICodeFormatter> _codeFormatter;
      private Mock<IImageProcessor> _imageProcessor;

      private string Publish(string html)
      {
         _codeFormatter = new Mock<ICodeFormatter>();
         _codeFormatter
          .Setup(x => x.Format(It.IsAny<string>(), It.IsAny<string>()))
          .Returns((string language, string code) => code);

         _imageProcessor = new Mock<IImageProcessor>();
         _imageProcessor
            .Setup(x => x.Minimize(It.IsAny<string>()))
            .Returns("minImage");

         var draft = new Draft();
         draft.Info = new PostInfo("the post");
         draft.Info.Summary = "summary";
         draft.Info.Tags = "tags";
         draft.Info.Language = Language.English;
         draft.Content = html;
         draft.RenderImages();
         return draft.Publish(DateTime.Now, _codeFormatter.Object, _imageProcessor.Object).Content;
      }

      private string Publish(params string[] htmlLines) =>
         Publish(htmlLines.JoinLines());

      [Fact]
      public void Return_the_same_for_not_known_elements() =>
          Publish("<span contenteditable=\"true\">TEXT</span><h3>H3</h3><h4>H4</h4>")
          .Should()
          .Be("<span>TEXT</span><h3>H3</h3><h4>H4</h4>");

      [Fact]
      public void Delete_empty_paragraphs() =>
          Publish("<p>1</p><p> </p><p>2</p>")
          .Should()
          .Be("<p>1</p><p>2</p>");

      [Fact]
      public void Remove_empty_spaces() =>
          Publish("<p> Hello </p>")
          .Should()
          .Be("<p>Hello</p>");

      [Fact]
      public void Wrap_codes() =>
           Publish("<pre class=\"code\">", "csharp, no-line-number", "<b>CODE</b></pre>")
          .Should()
          .Be("<div class=\"code\"><pre><b>CODE</b></pre></div>");

      [Fact]
      public void Format_code()
      {
         Publish(
            "<pre class=\"code\">js, no-line-number",
            "var a = 1;",
            "var b = 2;</pre>")
         .Should()
         .BeLines(
            "<div class=\"code\"><pre>var a = 1;",
            "var b = 2;</pre></div>");

         _codeFormatter.Verify(x => x.Format("js", string.Join(Environment.NewLine, "var a = 1;", "var b = 2;")));
      }

      [Fact]
      public void Set_line_numbers() =>
         Publish(
            "<pre class=\"code\">csharp",
            "var a = 12;",
            "var b = 13;</pre>")
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><table><tr><td>1",
            "2</td><td>var a = 12;",
            "var b = 13;</td></tr></table></pre></div>");

      [Fact]
      public void Highlight_marked_lines_with_line_numbers() =>
         Publish(
            "<pre class=\"code\">csharp",
            "var a = 12; #hl",
            "var b = 13;</pre>")
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><table><tr><td><span class=\"highlight\">1</span>",
            "2</td><td><span class=\"highlight\">var a = 12;</span>",
            "var b = 13;</td></tr></table></pre></div>");

      [Fact]
      public void Highlight_html() =>
         Publish("<pre class=\"code\">html, no-line-number", "<div>TEXT</div> #hl</pre>")
            .Should()
            .Be("<div class=\"code\"><pre><span class=\"highlight\"><div>TEXT</div></span></pre></div>");

      [Fact]
      public void Hightlight_marked_lines_without_line_numbers() =>
         Publish(
            "<pre class=\"code\">csharp, no-line-number",
            "var a = 12; #hl",
            "var b = 13;</pre>")
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><span class=\"highlight\">var a = 12;</span>",
            "var b = 13;</pre></div>");

      [Fact]
      public void Wrap_terminals() =>
          Publish("<pre class=\"terminal\"><b>CMD</b></pre>")
          .Should()
          .Be("<div class=\"cmd\"><pre><b>CMD</b></pre></div>");

      [Fact]
      public void Wrap_notes() =>
          Publish("<div class=\"note\"><b>NOTE</b></div>")
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>");

      [Fact]
      public void Wrap_warnings() =>
          Publish("<div class=\"warning\"><b>WARN</b></div>")
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>");

      [Fact]
      public void Unordered_lists() =>
          Publish("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>")
          .Should()
          .Be("<ul><li>I1</li><li><b>I2</b></li></ul>");

      [Fact]
      public void Ordered_lists() =>
          Publish("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>")
          .Should()
          .Be("<ol><li>I1</li><li><b>I2</b></li></ol>");

      [Fact]
      public void Dont_touch_insiders() =>
          Publish("<p contenteditable=\"true\"><strong>Hello</strong>World</p>")
          .Should()
          .Be("<p><strong>Hello</strong>World</p>");

      [Fact]
      public void Set_img_attributes() =>
          Publish("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption contenteditable=\"true\">CAP</figcaption></figure>")
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.png\" alt=\"CAP\"><figcaption>CAP</figcaption></figure>");

      [Fact]
      public void Figures_without_captions() =>
          Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>")
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.jpeg\"></figure>");

      [Fact]
      public void Figures_with_empty_captions() =>
          Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>")
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.jpeg\"></figure>");

   }
}
