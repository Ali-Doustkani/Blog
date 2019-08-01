using System.Collections.Generic;

namespace Blog.Domain.DeveloperStory
{
   public class Developer : DomainEntity
   {
      public Developer(string summary, string skills)
      {
         Summary = Its.NotEmpty(summary, nameof(Summary));
         Skills = Its.NotEmpty(skills, nameof(Skills));
         Experiences = new List<Experience>();
         SideProjects = new List<SideProject>();
      }

      public string Summary { get; private set; }
      public string Skills { get; private set; }
      public List<Experience> Experiences { get; }
      public List<SideProject> SideProjects { get; }
   }
}
