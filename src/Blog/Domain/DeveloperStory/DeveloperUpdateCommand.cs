using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Blog.Domain.DeveloperStory
{
   public class DeveloperUpdateCommand : Command<DeveloperUpdateCommand>
   {
      public DeveloperUpdateCommand()
      {
         _experiences = Enumerable.Empty<ExperienceEntry>();
         _sideProjects = Enumerable.Empty<SideProjectEntry>();
         _educations = Enumerable.Empty<EducationEntry>();
      }

      [Required]
      public string Summary { get; set; }

      [Required]
      public string Skills { get; set; }

      private IEnumerable<ExperienceEntry> _experiences;
      public IEnumerable<ExperienceEntry> Experiences
      {
         get => _experiences;
         set => _experiences = value ?? Enumerable.Empty<ExperienceEntry>();
      }

      private IEnumerable<SideProjectEntry> _sideProjects;
      public IEnumerable<SideProjectEntry> SideProjects
      {
         get => _sideProjects;
         set => _sideProjects = value ?? Enumerable.Empty<SideProjectEntry>();
      }

      private IEnumerable<EducationEntry> _educations;
      public IEnumerable<EducationEntry> Educations
      {
         get => _educations;
         set => _educations = value ?? Enumerable.Empty<EducationEntry>();
      }
   }
}
