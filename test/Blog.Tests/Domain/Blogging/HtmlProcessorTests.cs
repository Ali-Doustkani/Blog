using Blog.Domain.Blogging;
using Blog.Utils;
using FluentAssertions;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Domain.Blogging
{
   public class HtmlProcessorTests
   {
      private HtmlProcessor _processor;
      private ICodeFormatter _codeFormatter;
      private IImageProcessor _imageProcessor;

      private async Task<string> Publish(string html)
      {
         _codeFormatter = Substitute.For<ICodeFormatter>();
         _codeFormatter.FormatAsync(Arg.Any<string>(), Arg.Any<string>()).Returns(x => Task.FromResult((string)x[1]));

         _imageProcessor = Substitute.For<IImageProcessor>();
         _imageProcessor.Minimize(Arg.Any<string>()).Returns("minImage");

         _processor = new HtmlProcessor(_codeFormatter, _imageProcessor);
         var renderer = new ImageRenderer("the-post");
         var result = renderer.Render(html);
         return await _processor.ProcessAsync(result.Html);
      }

      private async Task<string> Publish(params string[] htmlLines) =>
         await Publish(htmlLines.JoinLines());

      [Fact]
      public async Task Return_the_same_for_not_known_elements() =>
          (await Publish("<span contenteditable=\"true\">TEXT</span><h3>H3</h3><h4>H4</h4>"))
          .Should()
          .Be("<span>TEXT</span><h3>H3</h3><h4>H4</h4>");

      [Fact]
      public async Task Delete_empty_paragraphs() =>
          (await Publish("<p>1</p><p> </p><p>2</p>"))
          .Should()
          .Be("<p>1</p><p>2</p>");

      [Fact]
      public async Task Remove_empty_spaces() =>
          (await Publish("<p> Hello </p>"))
          .Should()
          .Be("<p>Hello</p>");

      [Fact]
      public async Task Wrap_codes() =>
           (await Publish("<pre class=\"code\">", "csharp, no-line-number", "<b>CODE</b></pre>"))
          .Should()
          .Be("<div class=\"code\"><pre><b>CODE</b></pre></div>");

      [Fact]
      public async Task Format_code()
      {
         (await Publish(
            "<pre class=\"code\">js, no-line-number",
            "var a = 1;",
            "var b = 2;</pre>"))
         .Should()
         .BeLines(
            "<div class=\"code\"><pre>var a = 1;",
            "var b = 2;</pre></div>");

         await _codeFormatter
            .Received()
            .FormatAsync("js", string.Join(Environment.NewLine, "var a = 1;", "var b = 2;"));
      }

      [Fact]
      public async Task Set_line_numbers() =>
         (await Publish(
            "<pre class=\"code\">csharp",
            "var a = 12;",
            "var b = 13;</pre>"))
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><table><tr><td>1",
            "2</td><td>var a = 12;",
            "var b = 13;</td></tr></table></pre></div>");

      [Fact]
      public async Task Highlight_marked_lines_with_line_numbers() =>
         (await Publish(
            "<pre class=\"code\">csharp",
            "var a = 12; #hl",
            "var b = 13;</pre>"))
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><table><tr><td><span class=\"highlight\">1</span>",
            "2</td><td><span class=\"highlight\">var a = 12;</span>",
            "var b = 13;</td></tr></table></pre></div>");

      [Fact]
      public async Task Highlight_html() =>
         (await Publish("<pre class=\"code\">html, no-line-number", "<div>TEXT</div> #hl</pre>"))
            .Should()
            .Be("<div class=\"code\"><pre><span class=\"highlight\"><div>TEXT</div></span></pre></div>");

      [Fact]
      public async Task Hightlight_marked_lines_without_line_numbers() =>
         (await Publish(
            "<pre class=\"code\">csharp, no-line-number",
            "var a = 12; #hl",
            "var b = 13;</pre>"))
         .Should()
         .BeLines(
            "<div class=\"code\"><pre><span class=\"highlight\">var a = 12;</span>",
            "var b = 13;</pre></div>");

      [Fact]
      public async Task Wrap_terminals() =>
          (await Publish("<pre class=\"terminal\"><b>CMD</b></pre>"))
          .Should()
          .Be("<div class=\"cmd\"><pre><b>CMD</b></pre></div>");

      [Fact]
      public async Task Wrap_notes() =>
          (await Publish("<div class=\"note\"><b>NOTE</b></div>"))
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"note\"><b>NOTE</b></span></div>");

      [Fact]
      public async Task Wrap_warnings() =>
          (await Publish("<div class=\"warning\"><b>WARN</b></div>"))
          .Should()
          .Be("<div class=\"box-wrapper\"><span class=\"warning\"><b>WARN</b></span></div>");

      [Fact]
      public async Task Unordered_lists() =>
          (await Publish("<ul><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ul>"))
          .Should()
          .Be("<ul><li>I1</li><li><b>I2</b></li></ul>");

      [Fact]
      public async Task Ordered_lists() =>
          (await Publish("<ol><li contenteditable=\"true\">I1</li><li contenteditable=\"true\"><b>I2</b></li></ol>"))
          .Should()
          .Be("<ol><li>I1</li><li><b>I2</b></li></ol>");

      [Fact]
      public async Task Dont_touch_insiders() =>
          (await Publish("<p contenteditable=\"true\"><strong>Hello</strong>World</p>"))
          .Should()
          .Be("<p><strong>Hello</strong>World</p>");

      [Fact]
      public async Task Set_img_attributes() =>
          (await Publish("<figure><button>Remove</button><img data-filename=\"pic.png\" src=\"data:image/png;base64,DATA\"><figcaption contenteditable=\"true\">CAP</figcaption></figure>"))
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.png\" alt=\"CAP\"><figcaption>CAP</figcaption></figure>");

      [Fact]
      public async Task Figures_without_captions() =>
          (await Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"></figure>"))
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.jpeg\"></figure>");

      [Fact]
      public async Task Figures_with_empty_captions() =>
          (await Publish("<figure><img data-filename=\"pic.jpeg\" src=\"data:image/jpeg;base64,DATA\"><figcaption></figcaption></figure>"))
          .Should()
          .BePath("<figure><img class=\"lazyload lazyloading\" src=\"minImage\" data-src=\"/images/posts/the-post/pic.jpeg\"></figure>");
   }
}
