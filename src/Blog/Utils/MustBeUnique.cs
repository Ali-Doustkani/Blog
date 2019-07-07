using Blog.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blog.Utils
{
    public class MustBeUnique : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            return ValidationResult.Success;
            //var title = (string)value;
            //var context = validationContext.GetService<BlogContext>();
            //if (context.Posts.Any(x => x.Id != ((PostViewModel)validationContext.ObjectInstance).Id && string.Equals(x.Title, title, StringComparison.OrdinalIgnoreCase)))
            //    return new ValidationResult("This title already exists in the database.");
            //return ValidationResult.Success;
        }
    }
}
