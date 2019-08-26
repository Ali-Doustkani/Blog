using Blog.Domain.DeveloperStory;
using FluentValidation;

namespace Blog.Validators.DeveloperStory
{
   public class SideProjectValidator : AbstractValidator<SideProjectEntry>
   {
      public SideProjectValidator()
      {
         RuleFor(x => x.Title).NotEmpty();
         RuleFor(x => x.Content).NotEmpty();
      }
   }
}
