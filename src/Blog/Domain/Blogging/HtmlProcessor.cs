﻿using Blog.Utils;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Blog.Domain.Blogging
{
   public interface IHtmlProcessor
   {
      string Process(string rawContent);
   }

   public class HtmlProcessor : IHtmlProcessor
   {
      public HtmlProcessor(ICodeFormatter codeFormatter, IImageProcessor imageProcessor)
      {
         _codeFormatter = codeFormatter;
         _imageProcessor = imageProcessor;
      }

      private readonly ICodeFormatter _codeFormatter;
      private readonly IImageProcessor _imageProcessor;

      public string Process(string rawContent)
      {
         var display = new StringBuilder(1000);
         var doc = new HtmlDocument();
         doc.LoadHtml(rawContent);
         doc.DocumentNode.ForEachChild(node =>
         {
            if (node.Is("pre.code"))
               display.Append(Code(node));
            else if (node.Is("pre.terminal"))
               display.Append(node.El("div.cmd>pre"));
            else if (node.Is("div.note"))
               display.Append(node.El("div.box-wrapper>span.note"));
            else if (node.Is("div.warning"))
               display.Append(node.El("div.box-wrapper>span.warning"));
            else if (node.Is("ul") || node.Is("ol"))
               display.Append(node.ElChildren());
            else if (node.Is("figure"))
               display.Append(Figure(node));
            else
               display.Append(node.El());
         });

         return display.ToString();
      }

      private string Code(HtmlNode node)
      {
         var plain = node.InnerHtml.Trim();
         var highlightedLines = GetHighlightedLines(GetCode(plain));
         var formattedCode = _codeFormatter.Format(
            GetLanguage(plain),
            GetCode(plain).ReplaceWithPattern(@"\s*#hl", string.Empty));

         var lineNumbers = string.Empty;
         var lines = formattedCode.SplitWithPattern(@"\r?\n");

         for (var i = 0; i < lines.Count(); i++)
         {
            var no = i + 1;
            if (highlightedLines.Contains(i))
            {
               lines[i] = Emmet.El("span.highlight", lines[i]);
               Increment(ref lineNumbers, Emmet.El("span.highlight", no));
            }
            else
            {
               Increment(ref lineNumbers, no);
            }
         }

         formattedCode = lines.JoinLines();

         if (GetDefinitionLine(plain).Contains("no-line-number"))
            return Emmet.El("div.code>pre", formattedCode);

         var table = $"<table><tr><td>{lineNumbers.Trim()}</td><td>{formattedCode}</td></tr></table>";
         return Emmet.El("div.code>pre", table);
      }

      private IEnumerable<int> GetHighlightedLines(string code) =>
         code
         .Split('\n')
         .Select((line, index) => new { line, index })
         .Where(x => x.line.Contains("#hl"))
         .Select(x => x.index);

      private void Increment(ref string lineNumbers, object value) =>
         lineNumbers = string.Concat(lineNumbers, Environment.NewLine, value);

      private static string GetDefinitionLine(string plain) =>
         plain.Substring(0, plain.IndexOf(Environment.NewLine));

      public static string GetLanguage(string plain) =>
         GetDefinitionLine(plain).Split(',').First();

      private string GetCode(string plain) =>
          HtmlEntity.DeEntitize(plain.Substring(plain.IndexOf(Environment.NewLine)).Trim());

      private string Figure(HtmlNode node)
      {
         var img = node.Child("img");
         if (Image.IsDataUrl(img.Attr("src")))
            throw new InvalidOperationException("<img> src is not rendered yet.");

         var src = img.Attr("src");
         img.Attributes.RemoveAll();
         img.SetAttributeValue("class", "lazyload lazyloading");
         img.SetAttributeValue("src", _imageProcessor.Minimize(src));
         img.SetAttributeValue("data-src", src);

         var caption = node.Child("figcaption");
         if (string.IsNullOrWhiteSpace(caption?.InnerHtml))
            return Emmet.El("figure", img.OuterHtml);

         if (caption.Attributes.Contains("contenteditable"))
            caption.Attributes["contenteditable"].Remove();

         img.SetAttributeValue("alt", caption.InnerText);

         return Emmet.El("figure", string.Join("", img.OuterHtml, caption.OuterHtml));
      }
   }
}