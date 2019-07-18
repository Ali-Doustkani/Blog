using Blog.Utils;

namespace Blog.Domain
{
   public class Problem
   {
      public Problem(string property, string message)
      {
         Property = property;
         Message = Its.NotEmpty(message, nameof(message));
      }

      public string Property { get; }
      public string Message { get; }
   }
}
