using System.Collections.Generic;

namespace Blog.Domain
{
   public class Developer : DomainEntity
   {
      public Developer()
      {
         Experiences = new List<WorkExperience>();
         SideProjects = new List<SideProject>();
      }

      public string Summary { get; set; }
      public List<WorkExperience> Experiences { get; }
      public List<SideProject> SideProjects { get; }
      public string Skills { get; set; }
   }
}
