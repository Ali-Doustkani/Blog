namespace Blog.Domain.DeveloperStory
{
   public class Education
   {
      private Education() { }

      public Education(int id, string degree, string university, Period period)
      {
         Id = id;
         Degree = Assert.NotNull(degree);
         University = Assert.NotNull(university);
         Period = Assert.NotNull(period);
      }

      public int Id { get; private set; }
      public string Degree { get; private set; }
      public string University { get; private set; }
      public Period Period { get; private set; }
   }
}
