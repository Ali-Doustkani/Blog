using Blog.Domain.Blogging;
using FluentAssertions;
using Xunit;

namespace Blog.Domain.Tests.Blogging
{
   public class EmmetTests
   {
      [Fact]
      public void SurroundTags() =>
          Emmet.El("p", "TEXT")
          .Should()
          .Be("<p>TEXT</p>");

      [Fact]
      public void SurroundWithClass() =>
          Emmet.El("p.style", "TEXT")
          .Should()
          .Be("<p class=\"style\">TEXT</p>");

      [Fact]
      public void CreateInsiderTags() =>
          Emmet.El("pre>code", "CODE")
          .Should()
          .Be("<pre><code>CODE</code></pre>");

      [Fact]
      public void CreateInsiderTagsWithClass() =>
           Emmet.El("pre>code.style", "CODE")
          .Should()
          .Be("<pre><code class=\"style\">CODE</code></pre>");

      [Fact]
      public void CreateTagsWithClass() =>
          Emmet.El("pre.s1>code.s2", "CODE")
          .Should()
          .Be("<pre class=\"s1\"><code class=\"s2\">CODE</code></pre>");
   }
}
