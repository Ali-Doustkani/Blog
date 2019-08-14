using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;

namespace Blog.Validation
{
   public class ValidationResponse
   {
      public ValidationResponse(string title, IEnumerable<ValidationError> errors)
      {
         Title = title;
         Errors = errors;
      }

      public string Title { get; }
      public IEnumerable<ValidationError> Errors { get; }

      public static ValidationResponse Create(ModelStateDictionary modelState)
      {
         return null;
      }
   }
}
