﻿using HtmlAgilityPack;
using System.Linq;

namespace Blog.Model
{
    public static class HtmlNodeExtensions
    {
        public static bool Is(this HtmlNode node, string selector)
        {
            if (!selector.Contains('.'))
                return node.OriginalName == selector;

            var all = selector.Split('.');
            var tag = all.ElementAt(0);
            var className = all.ElementAt(1);
            return node.OriginalName == tag && node.Attributes["class"].Value == className;
        }

        public static string El(this HtmlNode node) =>
            string.IsNullOrWhiteSpace(node.InnerHtml) ?
            string.Empty :
            Emmet.El(node.OriginalName, node.InnerHtml.Trim());

        public static string El(this HtmlNode node, string selector) =>
            Emmet.El(selector, node.InnerHtml.Trim());

        public static string ElChildren(this HtmlNode node) =>
             Emmet.El(node.OriginalName, string.Join("", node.ChildNodes.Select(El)));
    }
}
