using System.Collections.Generic;

namespace Blog.Services.Administrator
{
   public class Developer
   {
      public int Id { get; set; }
      public string Summary { get; set; }
      public List<Experience> Experiences { get; set; }
   }
}
