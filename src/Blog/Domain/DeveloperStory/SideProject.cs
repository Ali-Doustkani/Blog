namespace Blog.Domain.DeveloperStory
{
   public class SideProject : DomainEntity
   {
      public SideProject(int id, string title, string content)
      {
         Id = id;
         Title = Assert.Arg.NotNull(title);
         _content = Assert.Arg.NotNull(content);
      }

      private string _content;

      public string Title { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
