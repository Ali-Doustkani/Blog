using Blog.Services.DeveloperStory;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.DeveloperStory
{
   public class DeveloperUpdateCommand : Command<DeveloperUpdateCommand>
   {
      [Required]
      public string Summary { get; set; }

      [Required]
      public string Skills { get; set; }

      public IEnumerable<ExperienceEntry> Experiences { get; set; }

      public IEnumerable<SideProjectEntry> SideProjects { get; set; }

      public IEnumerable<EducationEntry> Educations { get; set; }
   }
}
