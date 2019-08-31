using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class ErrorManager
   {
      public ErrorManager()
      {
         _errors = new List<string>();
      }

      private readonly List<string> _errors;

      public ErrorManager Required(string value, string name)
      {
         if (string.IsNullOrWhiteSpace(value))
            _errors.Add($"'{name}' is required");
         return this;
      }

      public ErrorManager IfTrue(bool value, string message)
      {
         if (value)
            _errors.Add(message);
         return this;
      }

      public ErrorManager Add(string message)
      {
         _errors.Add(message);
         return this;
      }

      public ErrorManager Add(IEnumerable<string> errors)
      {
         _errors.AddRange(errors);
         return this;
      }

      public bool Dirty =>
         _errors.Any();

      public CommandResult ToResult() =>
         new CommandResult(_errors.ToArray());

      public IEnumerable<string> Errors =>
         _errors;
   }
}
