using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
   public class SaveResult
   {
      private SaveResult(bool failed, IEnumerable<Problem> problems, string url)
      {
         Failed = failed;
         Problems = problems;
         Url = url;
      }

      public bool Failed { get; }
      public IEnumerable<Problem> Problems { get; }
      public string Url { get; }

      public static SaveResult Failure(string message) =>
         new SaveResult(true, new[] { new Problem(string.Empty, message) }, null);

      public static SaveResult Failure(IEnumerable<Problem> problems) =>
         new SaveResult(true, problems, null);

      public static SaveResult Success(string url) =>
         new SaveResult(false, Enumerable.Empty<Problem>(), url);
   }
}
