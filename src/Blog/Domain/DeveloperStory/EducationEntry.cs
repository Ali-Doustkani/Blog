namespace Blog.Domain.DeveloperStory
{
   public class EducationEntry : DomainObjectEntry
   {
      public string Degree { get; set; }
      public string University { get; set; }
      public string StartDate { get; set; }
      public string EndDate { get; set; }
   }
}
