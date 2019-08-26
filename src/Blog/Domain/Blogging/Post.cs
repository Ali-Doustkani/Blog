using System;
using System.Globalization;

namespace Blog.Domain.Blogging
{
   public class Post : DomainEntity
   {
      public Post()
      {

      }

      public Post(DateTime publishDate)
      {
         PublishDate = publishDate;
      }

      public DateTime PublishDate { get; set; }

      public PostInfo Info { get; set; }

      public PostContent PostContent { get; set; }

      public string Url { get; set; }

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
