using Blog.Domain.DeveloperStory;
using Blog.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public enum Status
   {
      Created,
      Updated
   }

   public class SaveResult
   {
      public SaveResult(Status status, IEnumerable<int> experiences, IEnumerable<int> sideProjects)
      {
         Status = status;
         Experiences = Its.NotEmpty(experiences);
         SideProjects = Its.NotEmpty(sideProjects);
      }

      public SaveResult(Status status)
         : this(status, Enumerable.Empty<int>(), Enumerable.Empty<int>())
      { }

      public Status Status { get; }
      public IEnumerable<int> Experiences { get; }
      public IEnumerable<int> SideProjects { get; }

      public static SaveResult Created(Developer developer) =>
         Create(Status.Created, developer);

      public static SaveResult Updated(Developer developer) =>
         Create(Status.Updated, developer);

      private static SaveResult Create(Status operation, Developer developer) =>
         new SaveResult(operation, developer.Experiences.Select(x => x.Id), developer.SideProjects.Select(x => x.Id));
   }
}
