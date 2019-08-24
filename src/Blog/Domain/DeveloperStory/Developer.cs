using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class Developer
   {
      public Developer(string summary, string skills)
      {
         _summary = Its.NotEmpty(summary, nameof(summary));
         Skills = Its.NotEmpty(skills, nameof(Skills));
         _experiences = new List<Experience>();
         _sideProjects = new List<SideProject>();
         _educations = new List<Education>();
      }

      private string _summary;
      private readonly List<Experience> _experiences;
      private readonly List<SideProject> _sideProjects;
      private readonly List<Education> _educations;

      public int Id { get; private set; }
      public HtmlText Summary => new HtmlText(_summary);
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.OrderBy(x => x.StartDate).ToArray();
      public IReadOnlyCollection<SideProject> SideProjects => _sideProjects.ToArray();
      public IReadOnlyCollection<Education> Educations => _educations.OrderBy(x => x.Period.StartDate).ToArray();

      public void Update(string summary, string skills)
      {
         _summary = Its.NotEmpty(summary, nameof(summary));
         Skills = Its.NotEmpty(skills, nameof(Skills));
      }

      public IEnumerable<string> GetSkillLines() => Skills.Split('\n');

      public void AddExperience(string company, string position, DateTime startDate, DateTime endDate, string content) =>
         AddExperience(0, company, position, startDate, endDate, content);

      public void UpdateExperience(int id, string newCompany, string newPosition, DateTime newStartDate, DateTime newEndDate, string newContent)
      {
         RemoveExperience(_experiences.Single(x => x.Id == id));
         AddExperience(id, newCompany, newPosition, newStartDate, newEndDate, newContent);
      }

      public void RemoveExperience(Experience experience) =>
         _experiences.Remove(experience);

      public void AddSideProject(string title, string content) =>
         AddSideProject(0, title, content);

      public void UpdateSideProject(int id, string newTitle, string newContent)
      {
         RemoveSideProject(_sideProjects.Single(x => x.Id == id));
         AddSideProject(id, newTitle, newContent);
      }

      public void RemoveSideProject(SideProject sideProject) =>
         _sideProjects.Remove(sideProject);

      public void AddEducation(string degree, string university, DateTime startDate, DateTime endDate) =>
         AddEducation(0, degree, university, startDate, endDate);

      public void UpdateEducation(int id, string newDegree, string newUniversity, DateTime newStartDate, DateTime newEndDate)
      {
         RemoveEducation(_educations.Single(x => x.Id == id));
         AddEducation(id, newDegree, newUniversity, newStartDate, newEndDate);
      }

      public void RemoveEducation(Education education) =>
         _educations.Remove(education);

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

      private void AddEducation(int id, string degree, string university, DateTime startDate, DateTime endDate)
      {
         if (_educations.Any(x => x.Degree == degree && x.University == university))
            throw new DomainProblemException("Another education item with the same degree and university already exists");

         var period = new Period(startDate, endDate);
         if (_educations.Any(x => x.Period.Overlaps(period)))
            throw new DomainProblemException("Education items should not have date overlaps with each other");

         _educations.Add(new Education(id, degree, university, period));
      }
   }
}
