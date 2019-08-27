namespace Blog.Domain.DeveloperStory
{
   public class Experience : DomainEntity
   {
      private Experience() { }

      public Experience(int id, string company, string position, Period period, string content)
      {
         Id = id;
         Company = Assert.Arg.NotNull(company);
         Position = Assert.Arg.NotNull(position);
         Period = Assert.Arg.NotNull(period);
         _content = Assert.Arg.NotNull(content);
      }

      private string _content;

      public string Company { get; private set; }
      public string Position { get; private set; }
      public Period Period { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
