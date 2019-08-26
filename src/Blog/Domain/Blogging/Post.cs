using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class Post : DomainEntity
   {
      private Post() { }

      public Post(int id,
         string title,
         DateTime publishDate,
         Language language,
         string summary,
         string tags,
         string url,
         string content)
      {
         Id = id;
         Title = Assert.NotNull(title);
         PublishDate = publishDate;
         Language = language;
         Summary = Assert.NotNull(summary);
         Tags = Assert.NotNull(tags);
         Url = Assert.NotNull(url);
         Content = Assert.NotNull(content);
      }

      public string Title { get; private set; }

      public DateTime PublishDate { get; private set; }

      public Language Language { get; private set; }

      public string Summary { get; private set; }

      public string Tags { get; private set; }

      public string Content { get; private set; }

      public string Url { get; private set; }

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

      public static string ToLongPersianDate(DateTime date)
      {
         var cal = new PersianCalendar();
         var days = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهارشنبه", "پنج شنبه", "جمعه", "شنبه" };
         var dayName = days[(int)cal.GetDayOfWeek(date)];
         return $"{dayName}، {cal.GetDayOfMonth(date)} {MonthName(date)} {cal.GetYear(date)}";
      }

      public string GetLongPersianDate()
      {
         return ToLongPersianDate(PublishDate);
      }

      public string GetShortPersianDate()
      {
         var cal = new PersianCalendar();
         return $"{MonthName(PublishDate)} {cal.GetYear(PublishDate)}";
      }

      private static string MonthName(DateTime date)
      {
         var cal = new PersianCalendar();
         var months = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند" };
         return months[cal.GetMonth(date) - 1];
      }
   }
}
