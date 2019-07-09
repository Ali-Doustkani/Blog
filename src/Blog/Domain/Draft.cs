﻿using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class Draft
    {
        public Draft()
        {
            Tags = string.Empty;
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string UrlTitle { get; set; }

        public DateTime PublishDate { get; set; }

        public Language Language { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public bool Show { get; set; }

        public string Content { get; set; }

        public string DisplayContent { get; set; }

        public IEnumerable<string> GetTags()
        {
            if (string.IsNullOrEmpty(Tags))
                return Enumerable.Empty<string>();

            var result = new List<string>();
            foreach (var str in Tags.Split(","))
            {
                var trimmed = str.Trim();
                if (!string.IsNullOrEmpty(trimmed))
                    result.Add(trimmed);
            }
            return result;
        }

        public string GetLongPersianDate()
        {
            var cal = new PersianCalendar();
            var days = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه" };
            var dayName = days[(int)cal.GetDayOfWeek(PublishDate)];
            return $"{dayName}، {cal.GetDayOfMonth(PublishDate)} {MonthName()} {cal.GetYear(PublishDate)}";
        }

        public string GetShortPersianDate()
        {
            var cal = new PersianCalendar();
            return $"{MonthName()} {cal.GetYear(PublishDate)}";
        }

        private string MonthName()
        {
            var cal = new PersianCalendar();
            var months = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
            return months[cal.GetMonth(PublishDate) - 1];
        }

        public void PopulateUrlTitle()
        {
            if (!string.IsNullOrEmpty(Title))
                UrlTitle = Regex.Replace(Title, @"[\s.:]+", "-");
        }

        // Post Content

        public void Render()
        {
            if (string.IsNullOrEmpty(UrlTitle))
                throw new InvalidOperationException("Render display HTML needs UrlTitle. Populate it first.");

            _filenames = new Queue<string>();

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
                    display.Append(node.Figure(PostPath.ImageUrl(UrlTitle, GenerateFilename(node))));
                else
                    display.Append(node.El());
            });

            DisplayContent = display.ToString();
        }

        public IEnumerable<Image> GetImages()
        {
            if (string.IsNullOrEmpty(UrlTitle))
                throw new InvalidOperationException("Creating images needs UrlTitle. Populate it first.");

            if (_filenames == null)
                Render();

            var images = new List<Image>();
            var doc = new HtmlDocument();
            doc.LoadHtml(Content);
            doc.DocumentNode.ForEachChild(node =>
            {
                if (node.Is("figure"))
                    images.Add(Image(node, Path.Combine(UrlTitle, _filenames.Peek())));
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

        private Queue<string> _filenames;

        private string GenerateFilename(HtmlNode node)
        {
            var img = node.Child("img");

            var dataFilename = img.Attr("data-filename");
            if (string.IsNullOrEmpty(dataFilename))
                throw new InvalidOperationException("data-filename attribute is not set for <img>");

            img.Attributes["data-filename"].Remove();

            while (_filenames.Contains(dataFilename))
                dataFilename = PostPath.Increment(dataFilename);

            _filenames.Enqueue(dataFilename);
            return dataFilename;
        }
    }
}