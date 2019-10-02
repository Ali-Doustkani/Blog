using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests
{
   public class HtmlTextTests
   {
      [Fact]
      public void Removes_attributes_from_raw_html_text()
      {
         var html = new HtmlText("<p contenteditable class=\"CLS\">Hello</p>");
         html.Content.Should().Be("<p>Hello</p>");
      }

      [Fact]
      public void Returns_empty_string_when_raw_content_is_empty()
      {
         var html = new HtmlText("");
         html.Content.Should().BeEmpty();
      }

      [Fact]
      public void Returns_empty_string_when_raw_content_is_null()
      {
         var html = new HtmlText(null);
         html.Content.Should().BeEmpty();
      }

      [Fact]
      public void Returns_empty_for_white_spaces()
      {
         var html = new HtmlText("  ");
         html.Content.Should().BeEmpty();
      }
   }
}
