using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.IO;

namespace Blog.Domain
{
   public static class Extensions
   {
      public static string ReplaceWithPattern(this string value, string pattern, string replacement) =>
         Regex.Replace(value, pattern, replacement);

      public static string[] SplitWithPattern(this string value, string pattern) =>
         Regex.Split(value, pattern);

      public static string ThrowIfNullOrEmpty<T>(this string value, string message)
      {
         Exception exc = new ArgumentNullException(message);
         if (typeof(T) == typeof(InvalidOperationException))
            exc = new InvalidOperationException(message);

         if (string.IsNullOrEmpty(value))
            throw exc;

         return value;
      }

      public static T Single<T>(this List<T> list, string id)
         where T : DomainEntity =>
         list.Single(x => x.Id.ToString() == id);

      public static string JoinLines(this string[] lines) =>
        string.Join(Environment.NewLine, lines);

      private const string PATTERN = @"(?<!<)[/\\]";

      public static string LocalPath(this string path) =>
           Regex.Replace(path, PATTERN, Path.DirectorySeparatorChar.ToString());

      public static string StandardPath(this string path) =>
         Regex.Replace(path, PATTERN, "/");
   }
}
