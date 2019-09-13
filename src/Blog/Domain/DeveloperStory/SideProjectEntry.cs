using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.DeveloperStory
{
   public class SideProjectEntry : DomainObjectEntry
   {
      [Required]
      public string Title { get; set; }

      [Required]
      public string Content { get; set; }
   }
}
