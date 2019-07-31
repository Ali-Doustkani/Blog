using System.Collections.Generic;

namespace Blog.Services.DeveloperStory
{
   public class DeveloperEntry
   {
      public string Summary { get; set; }
      public string Skills { get; set; }
      public IEnumerable<ExperienceEntry> Experiences { get; set; }
      public IEnumerable<SideProjectEntry> SideProjects { get; set; }
   }
}
