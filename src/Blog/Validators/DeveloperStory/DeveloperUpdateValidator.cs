using Blog.Domain.DeveloperStory;
using FluentValidation;
using System.Linq;

namespace Blog.Validators.DeveloperStory
{
   public class DeveloperUpdateValidator : AbstractValidator<DeveloperUpdateCommand>
   {
      public DeveloperUpdateValidator()
      {
         RuleFor(x => x.Summary).NotEmpty();
         RuleFor(x => x.Skills).NotEmpty();
         // RuleFor(x => x.Experiences).Must(x => x.Any()).WithMessage("Please provide at least one experience");
      }
   }
}
