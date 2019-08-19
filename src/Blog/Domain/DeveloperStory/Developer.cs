using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class Developer
   {
      public Developer(string summary, string skills)
      {
         Summary = Its.NotEmpty(summary, nameof(Summary));
         Skills = Its.NotEmpty(skills, nameof(Skills));
         _experiences = new List<Experience>();
         _sideProjects = new List<SideProject>();
      }

      private readonly List<Experience> _experiences;
      private readonly List<SideProject> _sideProjects;

      public int Id { get; private set; }
      public string Summary { get; private set; }
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.OrderBy(x => x.StartDate).ToArray();
      public IReadOnlyCollection<SideProject> SideProjects => _sideProjects.ToArray();

      public void Update(string summary, string skills)
      {
         Summary = Its.NotEmpty(summary, nameof(Summary));
         Skills = Its.NotEmpty(skills, nameof(Skills));
      }

      public void AddExperience(string company, string position, DateTime startDate, DateTime endDate, string content) =>
         AddExperience(0, company, position, startDate, endDate, content);

      public void UpdateExperience(int id, string newCompany, string newPosition, DateTime newStartDate, DateTime newEndDate, string newContent)
      {
         RemoveExperience(_experiences.Single(x => x.Id == id));
         AddExperience(id, newCompany, newPosition, newStartDate, newEndDate, newContent);
      }

      public void RemoveExperience(Experience experience)
      {
         _experiences.Remove(experience);
      }

      public void AddSideProject(string title, string content) =>
         AddSideProject(0, title, content);

      public void UpdateSideProject(int id, string newTitle, string newContent)
      {
         RemoveSideProject(_sideProjects.Single(x => x.Id == id));
         AddSideProject(id, newTitle, newContent);
      }

      public void RemoveSideProject(SideProject sideProject)
      {
         _sideProjects.Remove(sideProject);
      }

      private void AddExperience(int id, string company, string position, DateTime startDate, DateTime endDate, string content)
      {
         if (_experiences.Any(x => x.Company == company && x.Position == position))
            throw new DomainProblemException($"An experience of {position} at {company} already exists");

         if (_experiences.Any(x => (startDate > x.StartDate && startDate < x.EndDate) || (endDate > x.StartDate && endDate < x.EndDate)))
            throw new DomainProblemException("Experiences cannot have time overlaps with eachothers");

         _experiences.Add(new Experience(id, company, position, startDate, endDate, content));
      }

      private void AddSideProject(int id, string title, string content)
      {
         if (_sideProjects.Any(x => x.Title == title))
            throw new DomainProblemException("Title", $"The '{title}' project already exists");
         _sideProjects.Add(new SideProject(id, title, content));
      }
   }
}
