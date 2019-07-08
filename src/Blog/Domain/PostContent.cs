using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class PostContent
    {
        public int Id { get; set; }

        public string MarkedContent { get; set; }

        public string DisplayContent { get; set; }

        public IEnumerable<Image> Render(string urlTitle)
        {
            if (string.IsNullOrEmpty(urlTitle))
                throw new ArgumentNullException(nameof(urlTitle));

            var result = new List<Image>();
            var display = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(MarkedContent);
            var node = doc.DocumentNode.FirstChild;
            while (node != null)
            {
                if (node.Is("pre.code"))
                    display.Append(node.El("div.code>pre"));
                else if (node.Is("pre.terminal"))
                    display.Append(node.El("div.cmd>pre"));
                else if (node.Is("div.note"))
                    display.Append(node.El("div.box-wrapper>span.note"));
                else if (node.Is("div.warning"))
                    display.Append(node.El("div.box-wrapper>span.warning"));
                else if (node.Is("ul") || node.Is("ol"))
                    display.Append(node.ElChildren());
                else if (node.Is("figure"))
                {
                    var fullname = Path.Combine("images", "posts", urlTitle, Filename(node));
                    result.Add(Image(node, fullname));
                    display.Append(
                        node.Figure(
                            Path.Combine(Path.DirectorySeparatorChar.ToString(), fullname)
                            ));
                }
                else
                    display.Append(node.El());
                node = node.NextSibling;
            }
            DisplayContent = display.ToString();
            return result;
        }

        private Image Image(HtmlNode node, string imagePath)
        {
            var img = node.Child("img");
            if (img == null)
                throw new InvalidOperationException("The <figure> does not contain any <img>");

            if (!img.Attributes.Contains("src"))
                throw new InvalidOperationException("<img> must have src attribute in order to read the image.");

            return new Image(Path.Combine("wwwroot", imagePath), Data(img));
        }

        private byte[] Data(HtmlNode img)
        {
            var src = img.Attributes["src"];
            if (!Regex.IsMatch(src.Value, @"^data:image/(?:[a-zA-Z]+);base64,[a-zA-Z0-9+/]+=*$"))
                throw new InvalidOperationException($"src must have the Data URL pattern. DataURL: {src.Value}");

            var url = Regex.Match(src.Value, @",(?<data>.*)");
            var base64Data = url.Groups["data"].Value;
            return Convert.FromBase64String(base64Data);
        }

        private string Filename(HtmlNode node)
        {
            var img = node.Child("img");
            var src = img.Attributes["src"].Value;
            var extension =
                string.Concat(".",
                    Regex
                    .Match(src, @"data:image/(?<type>.*),")
                    .Groups["type"]
                    .Value
                    .Split(';')
                    .First());

            var dataFilename = img.Attributes["data-filename"]?.Value;
            if (!string.IsNullOrEmpty(dataFilename))
            {
                img.Attributes["data-filename"].Remove();
                return dataFilename;
            }

            return Path.ChangeExtension(Path.GetRandomFileName(), extension);
        }
    }
}
