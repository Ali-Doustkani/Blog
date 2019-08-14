using System.Collections.Generic;

namespace Blog.Validation
{
   public class ValidationError
   {
      public ValidationError(ValidationErrorType error, IEnumerable<object> paths)
      {
         Error = error;
         Paths = paths;
      }

      public ValidationErrorType Error { get; }
      public IEnumerable<object> Paths { get; }
   }
}
