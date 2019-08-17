using System.ComponentModel.DataAnnotations;

namespace Blog.Services.DeveloperStory
{
   public class SideProjectEntry
   {
      [Required]
      public string Id { get; set; }

      [Required]
      public string Title { get; set; }

      [Required]
      public string Content { get; set; }
   }
}
