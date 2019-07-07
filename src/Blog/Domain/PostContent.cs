using Blog.Utils;
using HtmlAgilityPack;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    public class PostContent
    {
        public int Id { get; set; }

        public string MarkedContent { get; set; }

        public string DisplayContent { get; set; }

        public void Render(IImageSaver imageSaver, string urlTitle)
        {
            var result = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(MarkedContent);
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
                    result.Append(Figure(node, imageSaver, urlTitle));
                else
                    result.Append(node.El());
                node = node.NextSibling;
            }
            DisplayContent = result.ToString();
        }

        private string Figure(HtmlNode node, IImageSaver imageSaver, string urlTitle)
        {
            var img = node.SelectNodes("//img").Single();
            if (!img.Attributes.Contains("src"))
                throw new System.Exception("There is not 'src' for the <img>");
            img.Attributes["src"].Value = imageSaver.Save(urlTitle, img);
            var caption = node.SelectNodes("//figcaption").Single();
            return Emmet.El("figure", string.Join("", img.OuterHtml, caption.OuterHtml));
        }
    }
}
