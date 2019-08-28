using System.Collections.Generic;

namespace Blog.CQ.PostListQuery
{
   public class PostItem
   {
      public string Title { get; set; }
      public string Url { get; set; }
      public string Date { get; set; }
      public string Summary { get; set; }
      public IEnumerable<string> Tags { get; set; }
   }
}
