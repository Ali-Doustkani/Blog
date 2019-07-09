using Blog.Domain;
using Xunit;

namespace Blog.Tests.Domain
{
    public class PostTests
    {
        [Theory]
        [InlineData("پیش مقدمه", "پیش-مقدمه")]
        [InlineData("پیش      مقدمه", "پیش-مقدمه")]
        [InlineData("یادگیری: ASP.NET Core", "یادگیری-ASP-NET-Core")]
        public void PopulateUrlTitle(string title, string result)
        {
            var draft = new Draft();
            draft.Info = new PostInfo();
            draft.Info.Title = title;
            draft.PopulateUrlTitle();

            Assert.Equal(result, draft.UrlTitle);
        }
    }
}
