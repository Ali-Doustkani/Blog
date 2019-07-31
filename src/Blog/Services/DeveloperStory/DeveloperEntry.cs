using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Services.DeveloperStory
{
   public class DeveloperEntry
   {
      [Required]
      public string Summary { get; set; }

      [Required]
      public string Skills { get; set; }

      public IEnumerable<ExperienceEntry> Experiences { get; set; }

      public IEnumerable<SideProjectEntry> SideProjects { get; set; }
   }
}
