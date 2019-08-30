using Blog.Services.DeveloperSaveCommand;
using FluentValidation;
using System.Linq;

namespace Blog.Validators.DeveloperStory
{
   public class DeveloperUpdateValidator : AbstractValidator<DeveloperSaveCommand>
   {
      public DeveloperUpdateValidator()
      {
         RuleFor(x => x.Summary).NotEmpty();
         RuleFor(x => x.Skills).NotEmpty();
         RuleFor(x => x.Experiences).Must(x => x.Any()).WithMessage("At least one experience must be provided.");
      }
   }
}
