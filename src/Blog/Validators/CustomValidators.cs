using FluentValidation;
using System;

namespace Blog.Validators
{
   public static class CustomValidators
   {
      public static IRuleBuilderOptions<T, string> BeDate<T>(this IRuleBuilder<T, string> ruleBuilder) =>
         ruleBuilder.Must(d => DateTime.TryParse(d, out DateTime val)).WithMessage(Messages.DATE_PARSE);

      public static bool IsDate(string date) =>
         DateTime.TryParse(date, out DateTime val);
   }
}
