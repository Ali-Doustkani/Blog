using System;

namespace Blog.Domain
{
   public class Problem
   {
      public Problem(string property, string message)
      {
         if (string.IsNullOrEmpty(message))
            throw new ArgumentNullException(nameof(message));

         Property = property;
         Message = message;
      }

      public string Property { get; }
      public string Message { get; }
   }
}
