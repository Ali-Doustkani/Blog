using System;
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

      public static DeveloperFactoryResult Create(DeveloperUpdateCommand command)
      {
         var dev = new Developer();
         var errorManager = new ErrorManager()
         .Add(dev.Update(command).Errors)
         .NotEmpty(command.Experiences, "Experiences");

         if (errorManager.Dirty)
            return DeveloperFactoryResult.MakeFailure(errorManager);

         return DeveloperFactoryResult.MakeSuccess(dev);
      }

      private string _summary;
      private readonly AggregateList<Experience> _experiences;
      private readonly AggregateList<SideProject> _sideProjects;
      private readonly AggregateList<Education> _educations;

      public HtmlText Summary => new HtmlText(_summary);
      public string Skills { get; private set; }
      public IReadOnlyCollection<Experience> Experiences => _experiences.ToArray();
      public IReadOnlyCollection<SideProject> SideProjects => _sideProjects.ToArray();
      public IReadOnlyCollection<Education> Educations => _educations.ToArray();

      public event OnUpdating Updating = delegate { };

      public IEnumerable<string> GetSkillLines() => Skills.Split('\n');

      public CommandResult Update(DeveloperUpdateCommand command)
      {
         if (command == null)
            throw new ArgumentNullException("command");

         var errorManager = Validate(command);
         if (errorManager.Dirty)
            return new CommandResult(errorManager.Errors);

         UpdateAggregates(command);
         return new CommandResult(Enumerable.Empty<string>());
      }

      private ErrorManager Validate(DeveloperUpdateCommand command)
      {
         return new ErrorManager()
         .NoDuplicate(command.Experiences,
            x => new { x.Position, x.Company },
            exp => $"The experience of '{exp.Position}' at '{exp.Company}' is duplicated")
         .Conditional(
            cond => cond.CheckPeriods(command.Experiences,
               x => x.StartDate,
               x => x.EndDate,
               exp => $"StartDate of '{exp.Company}' is greater than it's EndDate"),
            then => then.NoOverlaps(command.Experiences,
               x => x.StartDate,
               x => x.EndDate,
               (exp, others) => $"The experiences of '{exp.Company}' overlaps with '{string.Join(", ", others.Select(x => x.Company))}'"))
         .NoDuplicate(command.SideProjects,
            x => x.Title,
            proj => $"The side project of '{proj.Title}' is duplicated")
         .NoDuplicate(command.Educations,
            x => new { x.Degree, x.University },
            edu => $"The education of '{edu.Degree}' in '{edu.University}' is duplicated")
         .Conditional(
            cond => cond.CheckPeriods(command.Educations,
               x => x.StartDate,
               x => x.EndDate,
               edu => ""),
            then => then.NoOverlaps(command.Educations,
               x => x.StartDate,
               x => x.EndDate,
               (edu, others) => $"The education of '{edu.University}' overlaps with '{string.Join(", ", others.Select(x => x.University))}'"));

      }

      private void UpdateAggregates(DeveloperUpdateCommand command)
      {
         _summary = command.Summary;
         Skills = command.Skills;

         command.Update(_experiences, x => x.Experiences)
            .OnUpdate((id, exp) =>
            {
               var oldExperience = _experiences.Single(exp.Id);
               Updating(UpdatingType.Removing, oldExperience);
               _experiences.Remove(oldExperience);
               AddExperience(id, exp);
               Updating(UpdatingType.Added, _experiences.Single(exp.Id));
            })
            .OnAdd(exp =>
            {
               AddExperience(0, exp);
            });

         command.Update(_sideProjects, x => x.SideProjects)
            .OnUpdate((id, proj) =>
            {
               var toRemove = _sideProjects.Single(x => x.Id == id);
               Updating(UpdatingType.Removing, toRemove);
               _sideProjects.Remove(toRemove);
               AddSideProject(id, proj);
               Updating(UpdatingType.Added, _sideProjects.Single(proj.Id));
            })
            .OnAdd(proj =>
            {
               AddSideProject(0, proj);
            });

         command.Update(_educations, x => x.Educations)
             .OnUpdate((id, edu) =>
             {
                var toRemove = _educations.Single(edu.Id);
                Updating(UpdatingType.Removing, toRemove);
                _educations.Remove(toRemove);
                AddEducation(id, edu);
                Updating(UpdatingType.Added, _educations.Single(edu.Id));
             })
             .OnAdd(edu =>
             {
                AddEducation(0, edu);
             });
      }

      private void AddExperience(int id, ExperienceEntry exp) =>
         _experiences.Add(new Experience(id, exp.Company, exp.Position, Period.Parse(exp.StartDate, exp.EndDate), exp.Content));

      private void AddSideProject(int id, SideProjectEntry proj) =>
         _sideProjects.Add(new SideProject(id, proj.Title, proj.Content));

      private void AddEducation(int id, EducationEntry edu) =>
         _educations.Add(new Education(id, edu.Degree, edu.University, Period.Parse(edu.StartDate, edu.EndDate)));
   }
}
