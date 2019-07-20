using Blog.Utils;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Blog.Domain
{
   public class PostInfo : DomainEntity
   {
      public PostInfo()
      {
         Tags = string.Empty;
         Language = Language.English;
      }

      public string Title { get; set; }

      public string EnglishUrl { get; set; }

      public DateTime PublishDate { get; set; }

      public Language Language { get; set; }

      public string Summary { get; set; }

      public string Tags { get; set; }

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

      public string Slugify()
      {
         if (Language == Language.English)
         {
            if (!string.IsNullOrEmpty(EnglishUrl))
               return EnglishUrl;

            return SlugifyTitle();
         }

         if (string.IsNullOrEmpty(EnglishUrl))
            throw new InvalidOperationException("Enlish URL of Farsi posts must have value");
         return EnglishUrl;
      }

      private string SlugifyTitle() =>
         Title
         .ThrowIfNullOrEmpty<InvalidOperationException>("Publishing needs Title to be set.")
         .ToLower()
         .Replace("#", "sharp")
         .ReplaceWithPattern(@"[\s:/\\]+", "-")
         .ReplaceWithPattern(@"\.", "");
   }
}
