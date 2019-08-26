using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class CommandResult
   {
      public CommandResult()
      {
         _problems = new List<Error>();
      }

      public bool Failed => _problems.Any();

      private List<Error> _problems;

      public void AddError(string message) =>
         _problems.Add(new Error(message));

      public void AddError(string message, string property) =>
         _problems.Add(new Error(property, message));

      public IEnumerable<Error> Errors => _problems.ToArray();

      public static CommandResult Succeed => new CommandResult();
   }
}
