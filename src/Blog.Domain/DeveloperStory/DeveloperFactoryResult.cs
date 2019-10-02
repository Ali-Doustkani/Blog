using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class DeveloperFactoryResult : CommandResult
   {
      public DeveloperFactoryResult(IEnumerable<string> errors, Developer developer)
         : base(errors)
      {
         Developer = developer;
      }

      public Developer Developer { get; }

      public static DeveloperFactoryResult MakeFailure(ErrorManager errorManager) =>
         new DeveloperFactoryResult(errorManager.Errors, null);

      public static DeveloperFactoryResult MakeSuccess(Developer dev) =>
         new DeveloperFactoryResult(Enumerable.Empty<string>(), dev);
   }
}
