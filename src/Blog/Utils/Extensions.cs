using Blog.Domain;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;

namespace Blog.Utils
{
   public static class Extensions
   {
      public static bool IsMatch(this string value, string pattern) =>
         Regex.IsMatch(value, pattern);

      public static string ReplaceWithPattern(this string value, string pattern, string replacement) =>
         Regex.Replace(value, pattern, replacement);

      public static string[] SplitWithPattern(this string value, string pattern) =>
         Regex.Split(value, pattern);

      public static string ThrowIfNullOrEmpty(this string value, string message = null)
      {
         if (string.IsNullOrEmpty(value))
            throw new ArgumentNullException(message);
         return value;
      }

      public static string ThrowIfNullOrEmpty<T>(this string value, string message)
      {
         Exception exc = new ArgumentNullException(message);
         if (typeof(T) == typeof(InvalidOperationException))
            exc = new InvalidOperationException(message);

         if (string.IsNullOrEmpty(value))
            throw exc;

         return value;
      }

      public static void AddModelErrors(this ModelStateDictionary dic, IEnumerable<string> problems)
      {
         foreach (var prob in problems)
            dic.AddModelError(string.Empty, prob);
      }

      public static string JoinLines(this string[] lines) =>
         string.Join(Environment.NewLine, lines);

      public static T Single<T>(this List<T> list, string id)
        where T : DomainEntity =>
         list.Single(x => x.Id.ToString() == id);
   }
}
