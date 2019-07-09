﻿using HtmlAgilityPack;
using System.Collections.Generic;
using System.Text;

namespace Blog.Domain
{
    public class Draft
    {
        public int Id { get; set; }

        public PostInfo Info { get; set; }

        public bool Show { get; set; }

        public string Content { get; set; }

        public IEnumerable<Image> RenderImages()
        {
            var renderer = new ImageRenderer(Info.EncodeTitle());
            var result = renderer.Render(Content);
            Content = result.Html;
            return result.Images;
        }

        public Post Publish()
        {
            var display = new StringBuilder(1000);
            var doc = new HtmlDocument();
            doc.LoadHtml(Content);
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
                    display.Append(node.Figure());
                else
                    display.Append(node.El());
            });

            return new Post
            {
                Id = Id,
                Content = display.ToString(),
                Info = Info,
                UrlTitle = Info.EncodeTitle()
            };
        }
    }
}
