using System.Collections.Generic;

namespace Blog.Domain.DeveloperStory
{
   public class DeveloperUpdateCommand : Command<DeveloperUpdateCommand>
   {
      public DeveloperUpdateCommand()
      {
         _experiences = new List<ExperienceEntry>();
         _sideProjects = new List<SideProjectEntry>();
         _educations = new List<EducationEntry>();
      }

      public string Summary { get; set; }

      public string Skills { get; set; }

      private IList<ExperienceEntry> _experiences;
      public IList<ExperienceEntry> Experiences
      {
         get => _experiences;
         set => _experiences = value ?? new List<ExperienceEntry>();
      }

      private IList<SideProjectEntry> _sideProjects;
      public IList<SideProjectEntry> SideProjects
      {
         get => _sideProjects;
         set => _sideProjects = value ?? new List<SideProjectEntry>();
      }

      private IList<EducationEntry> _educations;
      public IList<EducationEntry> Educations
      {
         get => _educations;
         set => _educations = value ?? new List<EducationEntry>();
      }
   }
}
