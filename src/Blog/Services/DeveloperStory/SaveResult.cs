using Blog.Domain;
using Blog.Domain.DeveloperStory;
using Blog.Utils;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Services.DeveloperStory
{
   public enum Status
   {
      Created,
      Updated,
      Problem
   }

   public class SaveResult
   {
      public SaveResult(Status status, IEnumerable<int> experiences, IEnumerable<int> sideProjects)
      {
         Status = status;
         Experiences = experiences;
         SideProjects = sideProjects;
      }

      public SaveResult(Status status)
         : this(status, Enumerable.Empty<int>(), Enumerable.Empty<int>())
      { }

      public SaveResult(Problem problem)
      {
         Experiences = Enumerable.Empty<int>();
         SideProjects = Enumerable.Empty<int>();
         Status = Status.Problem;
         Problem = problem;
      }

      public Status Status { get; }
      public Problem Problem { get; }
      public IEnumerable<int> Experiences { get; }
      public IEnumerable<int> SideProjects { get; }

      public static SaveResult Problematic(DomainProblemException exception) =>
         new SaveResult(exception.Problem);

      public static SaveResult Created(Developer developer) =>
         Create(Status.Created, developer);

      public static SaveResult Updated(Developer developer) =>
         Create(Status.Updated, developer);

      private static SaveResult Create(Status operation, Developer developer) =>
         new SaveResult(operation, developer.Experiences.Select(x => x.Id), developer.SideProjects.Select(x => x.Id));
   }
}
