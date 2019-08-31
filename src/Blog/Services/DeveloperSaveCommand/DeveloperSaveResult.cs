using Blog.Domain;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DeveloperSaveCommand
{
   public class DeveloperSaveResult : CommandResult
   {
      public DeveloperSaveResult(bool created,
         IEnumerable<string> errors,
         IEnumerable<int> experiences,
         IEnumerable<int> sideProjects,
         IEnumerable<int> educations)
         : base(errors)
      {
         Created = created;
         Experiences = experiences;
         SideProjects = sideProjects;
         Educations = educations;
      }

      public bool Created { get; }
      public IEnumerable<int> Experiences { get; }
      public IEnumerable<int> SideProjects { get; }
      public IEnumerable<int> Educations { get; }

      public static DeveloperSaveResult MakeFailure(IEnumerable<string> errors) =>
         new DeveloperSaveResult(false, errors, Enumerable.Empty<int>(), Enumerable.Empty<int>(), Enumerable.Empty<int>());

      public static DeveloperSaveResult MakeSuccess(bool created, IEnumerable<int> experiences, IEnumerable<int> sideProjects, IEnumerable<int> educations) =>
         new DeveloperSaveResult(created, Enumerable.Empty<string>(), experiences, sideProjects, educations);
   }
}
