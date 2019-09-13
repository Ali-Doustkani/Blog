namespace Blog.Domain.DeveloperStory
{
   public class SideProject : DomainEntity
   {
      public SideProject(int id, string title, string content)
         : base(id)
      {
         Title = Assert.NotNull(title);
         _content = Assert.NotNull(content);
      }

      private string _content;

      public string Title { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
