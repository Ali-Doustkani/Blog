using System.Collections.Generic;

namespace Blog.Domain
{
   public class Developer : DomainEntity
   {
      public Developer()
      {
         Experiences = new List<Experience>();
         SideProjects = new List<SideProject>();
      }

      public string Summary { get; set; }
      public List<Experience> Experiences { get; }
      public List<SideProject> SideProjects { get; }
      public string Skills { get; set; }
   }
}
