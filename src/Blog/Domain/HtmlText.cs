using Blog.Domain.Blogging;
using HtmlAgilityPack;
using System.Text;

namespace Blog.Domain
{
   public class HtmlText
   {
      public HtmlText(string content)
      {
         RawContent = content;
      }

      public string RawContent { get; }

      public string Content
      {
         get
         {
            if (string.IsNullOrEmpty(RawContent))
               return string.Empty;

            var display = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(RawContent);
            doc.DocumentNode.ForEachChild(node => display.Append(node.El()));
            return display.ToString();
         }
      }
   }
}
