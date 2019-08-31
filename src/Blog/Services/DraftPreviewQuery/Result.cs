using Blog.Domain;
using Blog.Services.PostQuery;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DraftPreviewQuery
{
   public class Result : CommandResult
   {
      public Result(IEnumerable<string> errors, PostViewModel post)
         : base(errors)
      {
         Post = post;
      }

      public PostViewModel Post { get; }

      public static Result MakeFailure(CommandResult commandResult) =>
         new Result(commandResult.Errors, null);

      public static Result MakeSuccess(PostViewModel post) =>
         new Result(Enumerable.Empty<string>(), post);
   }
}
