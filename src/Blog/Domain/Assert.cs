using System;

namespace Blog.Domain
{
   public static class Assert
   {
      public static T NotNull<T>(T input, string name = null)
      {
         if (input == null)
            throw new ArgumentNullException(name);
         return input;
      }

      public static string NotNull(string input, string name = null)
      {
         if (string.IsNullOrWhiteSpace(input))
            throw new ArgumentNullException(name);
         return input;
      }
   }
}
