using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class CommandResult
   {
      public CommandResult(IEnumerable<Error> errors)
      {
         Errors = errors;
      }

      public IEnumerable<Error> Errors { get; }

      public bool Failed =>
         Errors.Any();
   }
}
