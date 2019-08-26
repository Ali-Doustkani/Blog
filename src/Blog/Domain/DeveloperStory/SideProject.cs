namespace Blog.Domain.DeveloperStory
{
   public class SideProject
   {
      public SideProject(int id, string title, string content)
      {
         Id = id;
         Title = Assert.NotNull(title);
         _content = Assert.NotNull(content);
      }

      private string _content;

      public int Id { get; private set; }
      public string Title { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
