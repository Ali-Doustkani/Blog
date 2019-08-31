using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DraftSaveCommand
{
   public class Result : CommandResult
   {
      public Result(IEnumerable<Error> errors, string postUrl)
         : base(errors)
      {
         PostUrl = postUrl;
      }

      public bool Published => PostUrl != null;
      public string PostUrl { get; }

      public static Result MakeFailure(CommandResult commandResult) =>
         new Result(commandResult.Errors, null);

      public static Result MakeFailure(string error) =>
         new Result(new[] { new Error(error) }, null);

      public static Result MakeSuccess() =>
         new Result(Enumerable.Empty<Error>(), null);

      public static Result MakeSuccess(string postUrl) =>
         new Result(Enumerable.Empty<Error>(), postUrl);
   }
}
