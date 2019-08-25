using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class CommandResult
   {
      public CommandResult()
      {
         _messages = new List<string>();
      }

      public bool Failed => _messages.Any();

      private List<string> _messages;

      public void AddError(string message) =>
         _messages.Add(message);

      public IEnumerable<string> Errors => _messages.ToArray();

      public static CommandResult Succeed => new CommandResult();
   }
}
