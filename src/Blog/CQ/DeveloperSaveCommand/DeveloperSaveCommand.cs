using Blog.Domain.DeveloperStory;
using MediatR;
using System.Collections.Generic;

namespace Blog.CQ.DeveloperSaveCommand
{
   public class DeveloperSaveCommand : IRequest<DeveloperSaveResult>
   {
      public string Summary { get; set; }
      public string Skills { get; set; }
      public IEnumerable<ExperienceEntry> Experiences { get; set; }
      public IEnumerable<SideProjectEntry> SideProjects { get; set; }
      public IEnumerable<EducationEntry> Educations { get; set; }
   }
}
