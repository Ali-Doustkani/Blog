namespace Blog.Domain.DeveloperStory
{
   public class SideProject : DomainEntity
   {
      public SideProject(string title, string content)
      {
         Title = Its.NotEmpty(title, nameof(Title));
         Content = Its.NotEmpty(content, nameof(Content));
      }

      public string Title { get; private set; }
      public string Content { get; private set; }
   }
}
