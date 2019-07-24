using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.Administrator
{
   public class SaveResult
   {
      private SaveResult(int id, bool failed, IEnumerable<Problem> problems, string url)
      {
         Id = id;
         Failed = failed;
         Problems = problems;
         Url = url;
      }

      public int Id { get; }
      public bool Failed { get; }
      public IEnumerable<Problem> Problems { get; }
      public string Url { get; }

      public static SaveResult Failure(int id, string message) =>
         new SaveResult(id, true, new[] { new Problem(string.Empty, message) }, null);

      public static SaveResult Failure(int id, IEnumerable<Problem> problems) =>
         new SaveResult(id, true, problems, null);

      public static SaveResult Success(int id, string url) =>
         new SaveResult(id, false, Enumerable.Empty<Problem>(), url);
   }
}
