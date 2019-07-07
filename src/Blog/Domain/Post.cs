using Blog.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Blog.Domain
{
    public class Post
    {
        public Post()
        {
            Tags = string.Empty;
            Content = new PostContent();
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public string UrlTitle { get; set; }

        public DateTime PublishDate { get; set; }

        public Language Language { get; set; }

        public string Summary { get; set; }

        public string Tags { get; set; }

        public bool Show { get; set; }

        public PostContent Content { get; set; }

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

        public void Render(IImageSaver imageSaver)
        {
            Content.Render(imageSaver, UrlTitle);
        }
    }
}
