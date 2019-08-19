namespace Blog.Domain.DeveloperStory
{
   public class SideProject
   {
      public SideProject(int id, string title, string content)
      {
         Id = id;
         Title = Its.NotEmpty(title, nameof(Title));
         Content = Its.NotEmpty(content, nameof(Content));
      }

      public int Id { get; private set; }
      public string Title { get; private set; }
      public string Content { get; private set; }
   }
}
