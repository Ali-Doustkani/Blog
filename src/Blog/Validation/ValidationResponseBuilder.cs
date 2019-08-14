using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blog.Validation
{
   public class ValidationResponseBuilder
   {
      private List<ValidationError> _types;

      public void BuildFrom(object model)
      {
         _types = new List<ValidationError>();
         var navigator = new PropertyNavigator(model);
         while (navigator.Read())
            navigator.CheckForEachAttribute(CheckRequired);
      }

      private void CheckRequired(Attribute attrib, Property property)
      {
         if (!(attrib is RequiredAttribute))
            return;

         if (property.Value is string)
         {
            if (string.IsNullOrWhiteSpace((string)property.Value))
               _types.Add(new ValidationError(ValidationErrorType.IsRequired, property.Path));
         }
         else if (property.Value is IEnumerable<object> d)
         {
            if (!d.Any())
               _types.Add(new ValidationError(ValidationErrorType.IsEmpty, property.Path));
         }
         else
         {
            if (property.Value == null)
               _types.Add(new ValidationError(ValidationErrorType.IsRequired, property.Path));
         }
      }

      public bool Invalid { get => _types.Any(); }

      public ValidationResponse Result
      {
         get => Invalid ? new ValidationResponse("Validation", _types) : null;
      }
   }
}
