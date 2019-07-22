using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain
{
   public class Draft : DomainEntity
   {
      public PostInfo Info { get; set; }

      public string Content { get; set; }

      public IEnumerable<Image> RenderImages()
      {
         var renderer = new ImageRenderer(Info.Slugify());
         var result = renderer.Render(Content);
         Content = result.Html;
         return result.Images;
      }

      /// <exception cref="CodeFormatException"/>
      public Post Publish(ICodeFormatter codeFormatter)
      {
         var display = new StringBuilder(1000);
         var doc = new HtmlDocument();
         doc.LoadHtml(Content);
         doc.DocumentNode.ForEachChild(node =>
        {
           if (node.Is("pre.code"))
              display.Append(Code(node, codeFormatter));
           else if (node.Is("pre.terminal"))
              display.Append(node.El("div.cmd>pre"));
           else if (node.Is("div.note"))
              display.Append(node.El("div.box-wrapper>span.note"));
           else if (node.Is("div.warning"))
              display.Append(node.El("div.box-wrapper>span.warning"));
           else if (node.Is("ul") || node.Is("ol"))
              display.Append(node.ElChildren());
           else if (node.Is("figure"))
              display.Append(node.Figure());
           else
              display.Append(node.El());
        });

         return new Post
         {
            Id = Id,
            PostContent = new PostContent { Id = Id, Content = display.ToString() },
            Info = Info,
            Url = Info.Slugify()
         };
      }

      private string Code(HtmlNode node, ICodeFormatter codeFormatter)
      {
         var plain = node.InnerHtml.Trim();
         var code = plain.Substring(plain.IndexOf(Environment.NewLine)).Trim();
         var formattedCode = codeFormatter.Format(GetLanguage(plain), HtmlEntity.DeEntitize(code)).Trim();

         if (GetDefinitionLine(plain).Contains("no-line-number"))
            return Emmet.El("div.code>pre", formattedCode);

         var lineNumbers = string.Empty;
         for (var i = 1; i <= formattedCode.Split('\n').Count(); i++)
            lineNumbers += Environment.NewLine + i;
         var table = $"<table><tr><td>{lineNumbers.Trim()}</td><td>{formattedCode}</td></tr></table>";
         return Emmet.El("div.code>pre", table);
      }

      private static string GetDefinitionLine(string plain) =>
         plain.Substring(0, plain.IndexOf(Environment.NewLine));

      public static string GetLanguage(string plain) =>
         GetDefinitionLine(plain).Split(',').First();
   }
}
