﻿using Blog.Domain.Blogging.Abstractions;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Domain.Blogging
{
   public interface IHtmlProcessor
   {
      /// <exception cref="ServiceDependencyException"/>
      Task<string> ProcessAsync(string rawContent);
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
      private StringBuilder _display;

      public async Task<string> ProcessAsync(string rawContent)
      {
         _display = new StringBuilder(1000);
         var doc = new HtmlDocument();
         doc.LoadHtml(rawContent);
         await doc.DocumentNode.ForEachChildAsync(async node =>
         {
            if (node.Is("pre.code"))
               _display.Append(await Code(node));
            else if (node.Is("pre.terminal"))
               _display.Append(node.El("div.cmd>pre"));
            else if (node.Is("div.note"))
               _display.Append(node.El("div.box-wrapper>span.note"));
            else if (node.Is("div.warning"))
               _display.Append(node.El("div.box-wrapper>span.warning"));
            else if (node.Is("ul") || node.Is("ol"))
               _display.Append(node.ElChildren());
            else if (node.Is("figure"))
               _display.Append(await FigureAsync(node));
            else
               _display.Append(node.El());
         });

         return _display.ToString();
      }

      private async Task<string> Code(HtmlNode node)
      {
         var plain = node.InnerHtml.Trim();
         var highlightedLines = GetHighlightedLines(GetCode(plain));
         var formattedCode = await _codeFormatter.FormatAsync(
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
         plain.Contains('\n') ? plain.Substring(0, plain.IndexOf('\n')) : plain;

      public static string GetLanguage(string plain) =>
         GetDefinitionLine(plain).Split(',').First();

      private string GetCode(string plain) =>
          HtmlEntity.DeEntitize(plain.Substring(plain.IndexOf(Environment.NewLine)).Trim());

      private async Task<string> FigureAsync(HtmlNode node)
      {
         var img = node.Child("img");
         if (Image.IsDataUrl(img.Attr("src")))
            throw new InvalidOperationException("<img> src is not rendered yet.");

         var src = img.Attr("src");
         img.Attributes.RemoveAll();
         img.SetAttributeValue("class", "lazyload lazyloading");
         img.SetAttributeValue("src", await _imageProcessor.Minimize(src));
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
