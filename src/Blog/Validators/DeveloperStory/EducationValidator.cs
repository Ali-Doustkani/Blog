using Blog.Domain.DeveloperStory;
using FluentValidation;
using System;

namespace Blog.Validators.DeveloperStory
{
   public class EducationValidator : AbstractValidator<EducationEntry>
   {
      public EducationValidator()
      {
         RuleFor(x => x.Degree).NotEmpty();
         RuleFor(x => x.University).NotEmpty();
         RuleFor(x => x.StartDate).BeDate();
         RuleFor(x => x.EndDate).BeDate();
         When(DatesAreOk, () =>
         {
            RuleFor(x => x.StartDate)
               .Must(BeLessThanEndDate).WithMessage(Messages.DATE_ORDER);
         });
      }

      private bool DatesAreOk(EducationEntry edu) =>
         CustomValidators.IsDate(edu.StartDate) && CustomValidators.IsDate(edu.EndDate);

      private bool BeLessThanEndDate(EducationEntry edu, string startDate) =>
         DateTime.Parse(edu.StartDate) < DateTime.Parse(edu.EndDate);
   }
}
