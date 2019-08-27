namespace Blog.Domain.DeveloperStory
{
   public class Education : DomainEntity
   {
      private Education() { }

      public Education(int id, string degree, string university, Period period)
      {
         Id = id;
         Degree = Assert.Arg.NotNull(degree);
         University = Assert.Arg.NotNull(university);
         Period = Assert.Arg.NotNull(period);
      }

      public string Degree { get; private set; }
      public string University { get; private set; }
      public Period Period { get; private set; }
   }
}
