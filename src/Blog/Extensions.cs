using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Blog
{
    public static class Extensions
    {
        public static string GetShortPersianDate(DateTime date)
        {
            var cal = new PersianCalendar();
            return $"{MonthName(cal.GetMonth(date))} {cal.GetYear(date)}";
        }

        public static string GetLongPersianDate(DateTime date)
        {
            var cal = new PersianCalendar();
            return $"{DayName(cal.GetDayOfWeek(date))}، {cal.GetDayOfMonth(date)} {MonthName(cal.GetMonth(date))} {cal.GetYear(date)}";
        }

        private static object DayName(DayOfWeek dayOfWeek)
        {
            switch (dayOfWeek)
            {
                case DayOfWeek.Friday: return "جمعه";
                case DayOfWeek.Monday: return "دوشنبه";
                case DayOfWeek.Saturday: return "شنبه";
                case DayOfWeek.Sunday: return "یکشنبه";
                case DayOfWeek.Thursday: return "پنج شنبه";
                case DayOfWeek.Tuesday: return "سه شنبه";
                case DayOfWeek.Wednesday: return "چهارشنبه";
            }
            throw new ArgumentException(nameof(dayOfWeek));
        }

        private static string MonthName(int month)
        {
            switch (month)
            {
                case 1: return "فروردین";
                case 2: return "اردیبهشت";
                case 3: return "خرداد";
                case 4: return "تیر";
                case 5: return "مرداد";
                case 6: return "شهریور";
                case 7: return "مهر";
                case 8: return "آبان";
                case 9: return "آذر";
                case 10: return "دی";
                case 11: return "بهمن";
                case 12: return "اسفند";
            }
            throw new ArgumentException(nameof(month));
        }

        public static string PopulateUrlTitle(string title)
        {
            return Regex.Replace(title, @"[\s.:]+", "-");
        }
    }
}
