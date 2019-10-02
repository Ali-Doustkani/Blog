using System;
using System.Collections.Generic;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class Developer : DomainEntity
   {
      private Developer()
         : base(0)
      {
         _experiences = new List<Experience>();
         _sideProjects = new List<SideProject>();
         _educations = new List<Education>();
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
      private readonly List<Experience> _experiences;
      private readonly List<SideProject> _sideProjects;
      private readonly List<Education> _educations;

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

         Update(_experiences, command.Experiences, AddExperience);
         Update(_sideProjects, command.SideProjects, AddSideProject);
         Update(_educations, command.Educations, AddEducation);
      }

      private void AddExperience(int id, ExperienceEntry exp) =>
         _experiences.Add(new Experience(id, exp.Company, exp.Position, Period.Parse(exp.StartDate, exp.EndDate), exp.Content));

      private void AddSideProject(int id, SideProjectEntry proj) =>
         _sideProjects.Add(new SideProject(id, proj.Title, proj.Content));

      private void AddEducation(int id, EducationEntry edu) =>
         _educations.Add(new Education(id, edu.Degree, edu.University, Period.Parse(edu.StartDate, edu.EndDate)));

      private void Update<TEntity, TEntry>(List<TEntity> entities, IEnumerable<TEntry> entries, Action<int, TEntry> adder)
         where TEntity : DomainEntity
         where TEntry : DomainObjectEntry
      {
         entities.RemoveAll(x => !entries.Select(y => y.Id).Contains(x.Id.ToString()));
         foreach (var item in entries)
         {
            if (int.TryParse(item.Id, out int id))
            {
               var oldEntity = entities.Single(item.Id);
               Updating(UpdatingType.Removing, oldEntity);
               entities.Remove(oldEntity);
               adder(id, item);
               Updating(UpdatingType.Added, entities.Single(item.Id));
            }
            else
            {
               adder(0, item);
            }
         }
      }
   }
}
