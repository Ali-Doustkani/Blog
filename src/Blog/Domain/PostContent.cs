﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class PostContent
    {
        public int Id { get; set; }

        public Post Post { get; set; }

        public string MarkedContent { get; set; }

        public string DisplayContent { get; set; }

        public void Render()
        {
            if (string.IsNullOrEmpty(Post.UrlTitle))
                throw new InvalidOperationException("Render display HTML needs UrlTitle. Populate it first.");

            var display = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(MarkedContent);
            doc.DocumentNode.ForEachChild(node =>
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
                    display.Append(node.Figure(PostPath.ImageUrl(Post.UrlTitle, GenerateFilename(node))));
                else
                    display.Append(node.El());
            });

            DisplayContent = display.ToString();
        }

        public IEnumerable<Image> GetImages()
        {
            if (string.IsNullOrEmpty(Post.UrlTitle))
                throw new InvalidOperationException("Creating images needs UrlTitle. Populate it first.");

            var images = new List<Image>();
            var doc = new HtmlDocument();
            doc.LoadHtml(MarkedContent);
            doc.DocumentNode.ForEachChild(node =>
            {
                if (node.Is("figure"))
                    images.Add(Image(node, Path.Combine(Post.UrlTitle, GenerateFilename(node))));
            });

            return images;
        }

        private Image Image(HtmlNode node, string imagePath)
        {
            var img = node.Child("img");
            if (img == null)
                throw new InvalidOperationException("The <figure> does not contain any <img>");

            if (!img.Attributes.Contains("src"))
                throw new InvalidOperationException("<img> must have src attribute in order to read the image.");

            var src = img.Attr("src");
            if (!Regex.IsMatch(src, @"^data:image/(?:[a-zA-Z]+);base64,[a-zA-Z0-9+/]+=*$"))
                throw new InvalidOperationException($"src must have the Data URL pattern. DataURL: {src}");

            var url = Regex.Match(src, @",(?<data>.*)");
            var base64Data = url.Groups["data"].Value;
            return new Image(imagePath, Convert.FromBase64String(base64Data));
        }

        private string GenerateFilename(HtmlNode node)
        {
            var img = node.Child("img");

            var dataFilename = img.Attr("data-filename");
            if (string.IsNullOrEmpty(dataFilename))
                throw new InvalidOperationException("data-filename attribute is not set for <img>");

            img.Attributes["data-filename"].Remove();
            return dataFilename;
        }
    }
}
