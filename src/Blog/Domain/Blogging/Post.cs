using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Blog.Domain.Blogging
{
   public class Post : DomainEntity
   {
      private Post() { }

      public Post(DateTime publishDate)
      {
         //Title = Assert.NotNull(title);
         PublishDate = publishDate;
      }

      public string Title { get; set; }

      public DateTime PublishDate { get; private set; }

      public string EnglishUrl { get; set; }

      public Language Language { get; set; }

      public string Summary { get; set; }

      public string Tags { get; set; }

      public PostContent PostContent { get; set; }

      public string Url { get; set; }

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
