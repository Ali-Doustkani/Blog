using FluentAssertions;
using FluentAssertions.Primitives;
using System.IO;
using System.Text.RegularExpressions;

namespace Blog.Tests
{
   public static class PathExtensions
   {
      private const string PATTERN = @"(?<!<)[/\\]";

      public static string Local(this string path) =>
           Regex.Replace(path, PATTERN, Path.DirectorySeparatorChar.ToString());

      public static AndConstraint<StringAssertions> BePath(this StringAssertions assertions, string path) =>
         assertions.Be(path.Local());
   }
}
