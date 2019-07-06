using Blog.Model;
using Xunit;

namespace Blog.Tests.Model
{
    public class EmmetTests
    {
        [Fact]
        public void SurroundTags()
        {
            Assert.Equal("<p>TEXT</p>", Emmet.El("p", "TEXT"));
        }

        [Fact]
        public void SurroundWithClass()
        {
            Assert.Equal("<p class=\"style\">TEXT</p>", Emmet.El("p.style", "TEXT"));
        }

        [Fact]
        public void CreateInsiderTags()
        {
            Assert.Equal("<pre><code>CODE</code></pre>", Emmet.El("pre>code", "CODE"));
        }

        [Fact]
        public void CreateInsiderTagsWithClass()
        {
            Assert.Equal("<pre><code class=\"style\">CODE</code></pre>", Emmet.El("pre>code.style", "CODE"));
        }

        [Fact]
        public void CreateTagsWithClass()
        {
            Assert.Equal("<pre class=\"s1\"><code class=\"s2\">CODE</code></pre>", Emmet.El("pre.s1>code.s2", "CODE"));
        }
    }
}
