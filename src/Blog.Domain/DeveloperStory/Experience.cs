namespace Blog.Domain.DeveloperStory
{
   public class Experience : DomainEntity
   {
      private Experience() : base(0) { }

      public Experience(int id, string company, string position, Period period, string content)
         : base(id)
      {
         Company = Assert.NotNull(company);
         Position = Assert.NotNull(position);
         Period = Assert.NotNull(period);
         _content = Assert.NotNull(content);
      }

      private string _content;

      public string Company { get; private set; }
      public string Position { get; private set; }
      public Period Period { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
