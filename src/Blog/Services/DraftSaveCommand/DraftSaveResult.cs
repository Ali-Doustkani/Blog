using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DraftSaveCommand
{
   public class DraftSaveResult : CommandResult
   {
      public DraftSaveResult(int id, IEnumerable<string> errors)
         : base(errors)
      {
         Id = id;
      }

      public int Id { get; }

      public static DraftSaveResult MakeFailure(int id, CommandResult commandResult) =>
        new DraftSaveResult(id, commandResult.Errors);

      public static DraftSaveResult MakeFailure(int id, string error) =>
         new DraftSaveResult(id, new[] { error });

      public static DraftSaveResult MakeSuccess(int id) =>
         new DraftSaveResult(id, Enumerable.Empty<string>());
   }
}
