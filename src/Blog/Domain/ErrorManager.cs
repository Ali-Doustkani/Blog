using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain
{
   public class ErrorManager
   {
      public ErrorManager()
      {
         _errors = new List<Error>();
      }

      private readonly List<Error> _errors;

      public ErrorManager Required(string value, string name)
      {
         if (string.IsNullOrWhiteSpace(value))
            _errors.Add(new Error($"'{name}' is required"));
         return this;
      }

      public ErrorManager IfTrue(bool value, string message)
      {
         if (value)
            _errors.Add(new Error(message));
         return this;
      }

      public ErrorManager Add(string message)
      {
         _errors.Add(new Error(message));
         return this;
      }

      public ErrorManager Add(IEnumerable<Error> errors)
      {
         _errors.AddRange(errors);
         return this;
      }

      public bool Dirty =>
         _errors.Any();

      public CommandResult ToResult() =>
         new CommandResult(_errors.ToArray());

      public IEnumerable<Error> Errors =>
         _errors;
   }
}
