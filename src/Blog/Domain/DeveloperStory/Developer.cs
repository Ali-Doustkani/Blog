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
         SideProjects = new List<SideProject>();
      }

      private readonly List<Experience> _experiences;

      public int Id { get; private set; }
      public string Summary { get; private set; }
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.ToArray();
      public List<SideProject> SideProjects { get; }

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

      private void AddExperience(int id, string company, string position, DateTime startDate, DateTime endDate, string content)
      {
         if (_experiences.Any(x => x.Company == company && x.Position == position))
            throw new DomainProblemException($"An experience of {position} at {company} already exists");

         if (_experiences.Any(x => x.EndDate > startDate))
            throw new DomainProblemException("Experiences cannot have time overlaps with eachothers");

         _experiences.Add(new Experience(id, company, position, startDate, endDate, content));
      }

      public void RemoveExperience(Experience experience)
      {
         _experiences.Remove(experience);
      }
   }
}
