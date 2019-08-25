using System;

namespace Blog.Domain
{
   public static class Assert
   {
      public static T NotNull<T>(T input)
      {
         if (input == null)
            throw new ArgumentNullException();
         return input;
      }
   }
}
