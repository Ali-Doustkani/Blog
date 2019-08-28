namespace Blog.CQ.DraftSaveCommand
{
   public class Result
   {
      public Result(string postUrl)
      {
         PostUrl = postUrl;
      }

      public bool Published => PostUrl != null;
      public string PostUrl { get; }
   }
}
