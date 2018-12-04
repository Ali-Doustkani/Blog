using Blog.Controllers;
using Xunit;

namespace Blog.Tests
{
    public class ExtensionsTests
    {
        [Fact]
        public void RemoveControllerWord()
        {
            Assert.Equal("Home", Extensions.NameOf<HomeController>());
        }
    }
}
