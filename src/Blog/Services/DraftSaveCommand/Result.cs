using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DraftSaveCommand
{
   public class Result
   {
      public Result(IEnumerable<Error> errors)
      {
         Errors = errors;
      }

      public Result(string postUrl)
      {
         PostUrl = postUrl;
         Errors = Enumerable.Empty<Error>();
      }

      public bool Published => PostUrl != null;
      public string PostUrl { get; }
      public IEnumerable<Error> Errors { get; }

      public static Result Failed(CommandResult commandResult) =>
         new Result(commandResult.Errors);

      public static Result Succeed() =>
         new Result(Enumerable.Empty<Error>());

      public static Result Succeed(string postUrl) =>
         new Result(postUrl);
   }
}
