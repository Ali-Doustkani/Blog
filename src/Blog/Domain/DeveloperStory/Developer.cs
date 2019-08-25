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
         _experiences = new AggregateList<Experience>(exp => exp.Id);
         _sideProjects = new AggregateList<SideProject>(proj => proj.Id);
         _educations = new AggregateList<Education>(edu => edu.Id);
      }

      private string _summary;
      private readonly AggregateList<Experience> _experiences;
      private readonly AggregateList<SideProject> _sideProjects;
      private readonly AggregateList<Education> _educations;

      public int Id { get; private set; }
      public HtmlText Summary => new HtmlText(_summary);
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.OrderByDescending(x => x.Period.StartDate).ToArray();
      public IReadOnlyCollection<SideProject> SideProjects => _sideProjects.ToArray();
      public IReadOnlyCollection<Education> Educations => _educations.OrderByDescending(x => x.Period.StartDate).ToArray();

      public IEnumerable<string> GetSkillLines() => Skills.Split('\n');

      public void Update(DeveloperUpdateCommand command, IStorageState storageState)
      {
         if (command == null)
            throw new ArgumentNullException(nameof(command));

         if (storageState == null)
            throw new ArgumentNullException(nameof(storageState));

         _summary = command.Summary;
         Skills = command.Skills;

         command.Update(_experiences, x => x.Experiences)
            .OnUpdate((id, exp) =>
            {
               var oldExperience = _experiences.Single(exp.Id);
               storageState.Detach(oldExperience.Period, oldExperience);
               _experiences.Remove(oldExperience);
               AddExperience(id, exp.Company, exp.Position, exp.GetStartDate(), exp.GetEndDate(), exp.Content);
               storageState.Modify(_experiences.Single(exp.Id).Period, _experiences.Single(exp.Id));
            })
            .OnAdd(exp =>
            {
               AddExperience(exp.Company, exp.Position, exp.GetStartDate(), exp.GetEndDate(), exp.Content);
            });

         command.Update(_sideProjects, x => x.SideProjects)
            .OnUpdate((id, proj) =>
            {
               storageState.Detach(_sideProjects.Single(proj.Id));
               _sideProjects.Remove(_sideProjects.Single(x => x.Id == id));
               AddSideProject(id, proj.Title, proj.Content);
               storageState.Modify(_sideProjects.Single(proj.Id));
            })
            .OnAdd(proj =>
            {
               AddSideProject(proj.Title, proj.Content);
            });

         command.Update(_educations, x => x.Educations)
             .OnUpdate((id, edu) =>
             {
                storageState.Detach(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
                _educations.Remove(_educations.Single(x => x.Id == id));
                AddEducation(id, edu.Degree, edu.University, edu.GetStartDate(), edu.GetEndDate());
                storageState.Modify(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
             })
             .OnAdd(edu =>
             {
                AddEducation(edu.Degree, edu.University, edu.GetStartDate(), edu.GetEndDate());
             });
      }

      public void AddExperience(string company, string position, DateTime startDate, DateTime endDate, string content) =>
         AddExperience(0, company, position, startDate, endDate, content);

      private void AddExperience(int id, string company, string position, DateTime startDate, DateTime endDate, string content)
      {
         if (_experiences.Any(x => x.Company == company && x.Position == position))
            throw new DomainProblemException($"An experience of {position} at {company} already exists");

         var period = new Period(startDate, endDate);
         if (_experiences.Any(x => x.Period.Overlaps(period)))
            throw new DomainProblemException("Experiences cannot have time overlaps with eachothers");

         _experiences.Add(new Experience(id, company, position, period, content));
      }

      public void AddSideProject(string title, string content) =>
         AddSideProject(0, title, content);

      private void AddSideProject(int id, string title, string content)
      {
         if (_sideProjects.Any(x => x.Title == title))
            throw new DomainProblemException("Title", $"The '{title}' project already exists");
         _sideProjects.Add(new SideProject(id, title, content));
      }

      public void AddEducation(string degree, string university, DateTime startDate, DateTime endDate) =>
         AddEducation(0, degree, university, startDate, endDate);

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
