using System.Collections.Generic;

namespace Blog.Services.Home
{
   public class DeveloperViewModel
   {
      public string Summary { get; set; }
      public IEnumerable<string> Skills { get; set; }
      public IEnumerable<ExperienceViewModel> Experiences { get; set; }
      public IEnumerable<SideProjectViewModel> SideProjects { get; set; }
      public IEnumerable<EducationViewModel> Educations { get; set; }
   }
}
