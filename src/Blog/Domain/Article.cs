using HtmlAgilityPack;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    public interface IImageService
    {
        string Save(string directory, string data);
    }

    public class Article
    {
        public Article(IImageService imageService)
        {
            _imageService = imageService;
        }

        readonly IImageService _imageService;

        public void Decorate(Post post)
        {
            var result = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(post.Content.MarkedContent);
            var node = doc.DocumentNode.FirstChild;
            while (node != null)
            {
                if (node.Is("pre.code"))
                    result.Append(node.El("div.code>pre"));
                else if (node.Is("pre.terminal"))
                    result.Append(node.El("div.cmd>pre"));
                else if (node.Is("div.note"))
                    result.Append(node.El("div.box-wrapper>span.note"));
                else if (node.Is("div.warning"))
                    result.Append(node.El("div.box-wrapper>span.warning"));
                else if (node.Is("ul") || node.Is("ol"))
                    result.Append(node.ElChildren());
                else if (node.Is("figure"))
                    Figure(post, node);
                else
                    result.Append(node.El());
                node = node.NextSibling;
            }

            post.Content.DisplayContent = result.ToString();
        }

        void Figure(Post post, HtmlNode node)
        {
            var img = node.SelectNodes("//img").Single();
            if (!img.Attributes.Contains("src"))
                throw new System.Exception("There is not 'src' for the <img>");
            var path = _imageService.Save(post.UrlTitle, img.Attributes["src"].Value);

            //select img
            //save it's content to file
            //use it's url for new 
        }
    }
}
