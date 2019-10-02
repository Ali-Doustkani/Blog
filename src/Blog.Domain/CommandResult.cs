using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class CommandResult
   {
      public CommandResult(IEnumerable<string> errors)
      {
         Errors = errors;
      }

      public IEnumerable<string> Errors { get; }

      public bool Failed =>
         Errors.Any();
   }
}
