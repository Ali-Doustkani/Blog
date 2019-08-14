using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Collections.Generic;

namespace Blog.Validation
{
   public class ValidationError
   {
      public ValidationError(ValidationErrorType error, IEnumerable<object> path)
      {
         Error = error;
         Path = path;
      }

      [JsonConverter(typeof(StringEnumConverter), true)]
      public ValidationErrorType Error { get; }

      public IEnumerable<object> Path { get; }
   }
}
