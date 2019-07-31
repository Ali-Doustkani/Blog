using System.Collections.Generic;

namespace Blog.Services.DeveloperStory
{
   public class SaveResult
   {
      public IEnumerable<int> Experiences { get; set; }
      public IEnumerable<int> SideProjects { get; set; }
   }
}
