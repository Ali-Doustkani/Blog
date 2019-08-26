using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class DeveloperUpdateCommandResult
   {
      public DeveloperUpdateCommandResult() { }

      public DeveloperUpdateCommandResult(IEnumerable<int> experiences,
         IEnumerable<int> sideProjects,
         IEnumerable<int> educations)
      {
         Experiences = experiences;
         SideProjects = sideProjects;
         Educations = educations;
      }

      public IEnumerable<int> Experiences { get; }
      public IEnumerable<int> SideProjects { get; }
      public IEnumerable<int> Educations { get; }

      public static DeveloperUpdateCommandResult Create(IEnumerable<Experience> experiences,
         IEnumerable<SideProject> sideProjects,
         IEnumerable<Education> educations) =>
         new DeveloperUpdateCommandResult(experiences.Select(x => x.Id),
            sideProjects.Select(x => x.Id),
            educations.Select(x => x.Id));
   }
}
