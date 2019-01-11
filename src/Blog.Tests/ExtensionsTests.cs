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

        [Fact]
        public void GetShortPersianDate()
        {
            Assert.Equal("دی 1397", Extensions.GetShortPersianDate(new System.DateTime(2018, 12, 25)));
        }

        [Fact]
        public void GetLongPersianDate()
        {
            Assert.Equal("سه شنبه، 4 دی 1397", Extensions.GetLongPersianDate(new System.DateTime(2018, 12, 25)));
        }

        [Theory]
        [InlineData("پیش مقدمه", "پیش-مقدمه")]
        [InlineData("پیش      مقدمه", "پیش-مقدمه")]
        [InlineData("یادگیری: ASP.NET Core", "یادگیری-ASP-NET-Core")]
        public void PopulateUrlTitle(string title, string result)
        {
            Assert.Equal(result, Extensions.PopulateUrlTitle(title));
        }
    }
}
