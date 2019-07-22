using HtmlAgilityPack;
using System;
using System.Collections.Generic;
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
         var language = plain.Substring(0, plain.IndexOf(Environment.NewLine));
         var code = plain.Substring(plain.IndexOf(Environment.NewLine)).Trim();
         return Emmet.El("div.code>pre", codeFormatter.Format(language, HtmlEntity.DeEntitize(code)));
      }
   }
}
