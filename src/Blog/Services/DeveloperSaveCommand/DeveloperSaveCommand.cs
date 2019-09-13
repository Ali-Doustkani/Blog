using Blog.Domain.DeveloperStory;
using MediatR;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Blog.Services.DeveloperSaveCommand
{
   public class DeveloperSaveCommand : IRequest<DeveloperSaveResult>
   {
      [Required]
      public string Summary { get; set; }

      [Required]
      public string Skills { get; set; }

      public IEnumerable<ExperienceEntry> Experiences { get; set; }
      public IEnumerable<SideProjectEntry> SideProjects { get; set; }
      public IEnumerable<EducationEntry> Educations { get; set; }
   }
}
