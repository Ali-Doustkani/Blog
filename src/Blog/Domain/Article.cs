using HtmlAgilityPack;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
    public class Article
    {
        public static string Decorate(string rawHtml)
        {
            var result = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(rawHtml);
            var node = doc.DocumentNode.FirstChild;
            while (node != null)
            {
                if (IsCode(node))
                    result.Append(Code(node));
                else if (IsTerminal(node))
                    result.Append(Terminal(node));
                else if (IsNote(node))
                    result.Append(Note(node));
                else if (IsWarning(node))
                    result.Append(Warning(node));
                else if (IsList(node))
                    result.Append(List(node));
                else
                    result.Append(TheSame(node));
                node = node.NextSibling;
            }
            return result.ToString();
        }

        private static bool IsCode(HtmlNode node) => node.OriginalName == "pre" && node.Attributes["class"].Value == "code";

        private static bool IsTerminal(HtmlNode node) => node.OriginalName == "pre" && node.Attributes["class"].Value == "terminal";

        private static bool IsNote(HtmlNode node) => node.OriginalName == "div" && node.Attributes["class"].Value == "note";

        private static bool IsWarning(HtmlNode node) => node.OriginalName == "div" && node.Attributes["class"].Value == "warning";

        private static bool IsList(HtmlNode node) => node.OriginalName == "ul" || node.OriginalName == "ol";

        private static string TheSame(HtmlNode node) =>
            (node.OriginalName == "p" && string.IsNullOrWhiteSpace(node.InnerHtml)) ?
            string.Empty :
            $"<{node.OriginalName}>{node.InnerHtml.Trim()}</{node.OriginalName}>";

        private static string Code(HtmlNode node) => $"<div class=\"code\"><pre>{node.InnerHtml}</pre></div>";

        private static string Terminal(HtmlNode node) => $"<div class=\"cmd\"><pre>{node.InnerHtml}</pre></div>";

        private static string Note(HtmlNode node) => $"<div class=\"box-wrapper\"><span class=\"note\">{node.InnerHtml}</span></div>";

        private static string Warning(HtmlNode node) => $"<div class=\"box-wrapper\"><span class=\"warning\">{node.InnerHtml}</span></div>";

        private static string List(HtmlNode node) => $"<{node.OriginalName}>{string.Join("", node.ChildNodes.Select(TheSame))}</{node.OriginalName}>";
    }
}
