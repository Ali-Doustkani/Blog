namespace Blog.Domain.DeveloperStory
{
   public class Experience : DomainEntity
   {
      private Experience() { }

      public Experience(int id, string company, string position, Period period, string content)
      {
         Id = id;
         Company = Its.NotEmpty(company, nameof(Company));
         Position = Its.NotEmpty(position, nameof(Position));
         Period = Its.NotEmpty(period, nameof(period));
         _content = Its.NotEmpty(content, nameof(content));
      }

      private string _content;

      public string Company { get; private set; }
      public string Position { get; private set; }
      public Period Period { get; private set; }
      public HtmlText Content => new HtmlText(_content);
   }
}
