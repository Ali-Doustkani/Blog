namespace Blog.Domain.DeveloperStory
{
   public class SideProject
   {
      public SideProject(int id, string title, string content)
      {
         Id = id;
         Title = Its.NotEmpty(title, nameof(Title));
         _content = Its.NotEmpty(content, nameof(content));
      }

      private string _content;

      public int Id { get; private set; }
      public string Title { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
