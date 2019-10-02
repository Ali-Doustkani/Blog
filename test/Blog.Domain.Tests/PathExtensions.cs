using FluentAssertions;
using FluentAssertions.Primitives;

namespace Blog.Domain.Tests
{
   public static class PathExtensions
   {
      public static AndConstraint<StringAssertions> BePath(this StringAssertions assertions, string path) =>
       assertions.Be(path.LocalPath());

      public static AndConstraint<StringAssertions> BeLines(this StringAssertions assertions, params string[] lines) =>
         assertions.Be(lines.JoinLines());
   }
}
