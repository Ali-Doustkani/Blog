namespace Blog.Domain.DeveloperStory
{
   public class ExperienceEntry : DomainObjectEntry
   {
      public string Company { get; set; }
      public string Position { get; set; }
      public string StartDate { get; set; }
      public string EndDate { get; set; }
      public string Content { get; set; }
   }
}
