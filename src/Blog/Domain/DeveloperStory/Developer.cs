﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class Developer : DomainEntity
   {
      private Developer()
      {
         _experiences = new AggregateList<Experience>();
         _sideProjects = new AggregateList<SideProject>();
         _educations = new AggregateList<Education>();
      }

      public Developer(string summary, string skills, IEnumerable<Experience> experiences)
        : this()
      {
         if (!experiences.Any())
            throw new ArgumentException("at least one experience is required");

         _summary = Assert.Arg.NotNull(summary);
         Skills = Assert.Arg.NotNull(skills);
         _experiences = new AggregateList<Experience>(experiences);
      }

      private string _summary;
      private readonly AggregateList<Experience> _experiences;
      private readonly AggregateList<SideProject> _sideProjects;
      private readonly AggregateList<Education> _educations;

      public HtmlText Summary => new HtmlText(_summary);
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.OrderByDescending(x => x.Period.StartDate).ToArray();
      public IReadOnlyCollection<SideProject> SideProjects => _sideProjects.ToArray();
      public IReadOnlyCollection<Education> Educations => _educations.OrderByDescending(x => x.Period.StartDate).ToArray();

      public IEnumerable<string> GetSkillLines() => Skills.Split('\n');

      public DeveloperUpdateCommandResult Update(DeveloperUpdateCommand command, IStorageState storageState)
      {
         Assert.Arg.NotNull(command);
         Assert.Arg.NotNull(storageState);

         UpdateAggregates(command, storageState);

         return DeveloperUpdateCommandResult.Create(_experiences, _sideProjects, _educations);
      }

      private void UpdateAggregates(DeveloperUpdateCommand command, IStorageState storageState)
      {
         _summary = command.Summary;
         Skills = command.Skills;

         command.Update(_experiences, x => x.Experiences)
            .OnUpdate((id, exp) =>
            {
               var oldExperience = _experiences.Single(exp.Id);
               storageState.Detach(oldExperience.Period, oldExperience);
               _experiences.Remove(oldExperience);
               AddExperience(id, exp);
               storageState.Modify(_experiences.Single(exp.Id).Period, _experiences.Single(exp.Id));
            })
            .OnAdd(exp =>
            {
               AddExperience(0, exp);
            });

         command.Update(_sideProjects, x => x.SideProjects)
            .OnUpdate((id, proj) =>
            {
               storageState.Detach(_sideProjects.Single(proj.Id));
               _sideProjects.Remove(_sideProjects.Single(x => x.Id == id));
               AddSideProject(id, proj);
               storageState.Modify(_sideProjects.Single(proj.Id));
            })
            .OnAdd(proj =>
            {
               AddSideProject(0, proj);
            });

         command.Update(_educations, x => x.Educations)
             .OnUpdate((id, edu) =>
             {
                storageState.Detach(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
                _educations.Remove(_educations.Single(x => x.Id == id));
                AddEducation(id, edu);
                storageState.Modify(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
             })
             .OnAdd(edu =>
             {
                AddEducation(0, edu);
             });
      }

      private void AddExperience(int id, ExperienceEntry exp)
      {
         if (_experiences.Any(x => x.Company == exp.Company && x.Position == exp.Position))
            throw new ArgumentException("Duplicate experience", $"{exp.Company}, {exp.Position}");

         var period = Period.Parse(exp.StartDate, exp.EndDate);
         if (_experiences.Any(x => x.Period.Overlaps(period)))
            throw new ArgumentException("Time overlaps in experience", $"{exp.Company}, {exp.Position}");

         _experiences.Add(new Experience(id, exp.Company, exp.Position, period, exp.Content));
      }

      private void AddSideProject(int id, SideProjectEntry proj)
      {
         if (_sideProjects.Any(x => x.Title == proj.Title))
            throw new ArgumentException("Duplicate side project", proj.Title);

         _sideProjects.Add(new SideProject(id, proj.Title, proj.Content));
      }

      private void AddEducation(int id, EducationEntry edu)
      {
         if (_educations.Any(x => x.Degree == edu.Degree && x.University == edu.University))
            throw new ArgumentException("Duplicate education", $"{edu.Degree}, {edu.University}");

         var period = Period.Parse(edu.StartDate, edu.EndDate);
         if (_educations.Any(x => x.Period.Overlaps(period)))
            throw new ArgumentException("Time overlaps in education", $"{edu.Degree}, {edu.University}");

         _educations.Add(new Education(id, edu.Degree, edu.University, period));
      }
   }
}
