using Blog.Domain.DeveloperStory;
using FluentValidation;
using System;

namespace Blog.Validators.DeveloperStory
{
   public class ExperienceValidator : AbstractValidator<ExperienceEntry>
   {
      public ExperienceValidator()
      {
         RuleFor(x => x.Company).NotEmpty();
         RuleFor(x => x.Position).NotEmpty();
         RuleFor(x => x.Content).NotEmpty();
         RuleFor(x => x.StartDate).BeDate();
         RuleFor(x => x.EndDate).BeDate();
         When(DatesAreOk, () =>
         {
            RuleFor(x => x.StartDate)
               .Must(BeLessThanEndDate).WithMessage(Messages.DATE_ORDER);
         });
      }

      private bool DatesAreOk(ExperienceEntry exp) =>
         CustomValidators.IsDate(exp.StartDate) && CustomValidators.IsDate(exp.EndDate);

      private bool BeLessThanEndDate(ExperienceEntry exp, string startDate) =>
         DateTime.Parse(exp.StartDate) < DateTime.Parse(exp.EndDate);
   }
}
