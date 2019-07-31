namespace Blog.Domain.Blogging
{
   public class Post : DomainEntity
   {
      public PostInfo Info { get; set; }

      public PostContent PostContent { get; set; }

      public string Url { get; set; }
   }
}
