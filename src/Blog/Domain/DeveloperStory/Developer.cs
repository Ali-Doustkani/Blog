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

      public DeveloperUpdateCommandResult Update(DeveloperUpdateCommand command, IStorageState storageState)
      {
         if (command == null)
            throw new ArgumentNullException(nameof(command));

         if (storageState == null)
            throw new ArgumentNullException(nameof(storageState));

         var result = new DeveloperUpdateCommandResult();

         foreach (var exp in command.Experiences)
         {
            if (exp.GetStartDate() >= exp.GetEndDate())
               result.AddError($"Start Date is greater than End Date for {exp.Position} at {exp.Company}");

            if (command.Experiences.Any(x => x.Id != exp.Id && x.Company == exp.Company && x.Position == exp.Position))
               result.AddError($"An experience of {exp.Position} at {exp.Company} already exists");

            if (command.Experiences.Any(x => x.Id != exp.Id && x.GetPeriod().Overlaps(exp.GetPeriod())))
               result.AddError("Experiences cannot have time overlaps with each other");
         }

         foreach (var proj in command.SideProjects)
         {
            if (command.SideProjects.Any(x => x.Id != proj.Id && x.Title == proj.Title))
               result.AddError($"The '{proj.Title}' project already exists");
         }

         foreach (var edu in command.Educations)
         {
            if (command.Educations.Any(x => x.Id != edu.Id && x.Degree == edu.Degree && x.University == edu.University))
               result.AddError("Another education item with the same degree and university already exists");

            if (command.Educations.Any(x => x.Id != edu.Id && x.GetPeriod().Overlaps(edu.GetPeriod())))
               result.AddError("Education items should not have date overlaps with each other");
         }

         if (result.Failed)
            return result;

         UpdateAggregates(command, storageState);

         return DeveloperUpdateCommandResult.Succeed(_experiences, _sideProjects, _educations);
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
               _experiences.Add(new Experience(id, exp.Company, exp.Position, exp.GetPeriod(), exp.Content));
               storageState.Modify(_experiences.Single(exp.Id).Period, _experiences.Single(exp.Id));
            })
            .OnAdd(exp =>
            {
               _experiences.Add(new Experience(0, exp.Company, exp.Position, exp.GetPeriod(), exp.Content));
            });

         command.Update(_sideProjects, x => x.SideProjects)
            .OnUpdate((id, proj) =>
            {
               storageState.Detach(_sideProjects.Single(proj.Id));
               _sideProjects.Remove(_sideProjects.Single(x => x.Id == id));
               _sideProjects.Add(new SideProject(id, proj.Title, proj.Content));
               storageState.Modify(_sideProjects.Single(proj.Id));
            })
            .OnAdd(proj =>
            {
               _sideProjects.Add(new SideProject(0, proj.Title, proj.Content));
            });

         command.Update(_educations, x => x.Educations)
             .OnUpdate((id, edu) =>
             {
                storageState.Detach(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
                _educations.Remove(_educations.Single(x => x.Id == id));
                _educations.Add(new Education(id, edu.Degree, edu.University, edu.GetPeriod()));
                storageState.Modify(_educations.Single(edu.Id).Period, _educations.Single(edu.Id));
             })
             .OnAdd(edu =>
             {
                _educations.Add(new Education(0, edu.Degree, edu.University, edu.GetPeriod()));
             });
      }
   }
}
