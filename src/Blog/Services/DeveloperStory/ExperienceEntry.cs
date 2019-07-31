using System.ComponentModel.DataAnnotations;

namespace Blog.Services.DeveloperStory
{
   public class ExperienceEntry
   {
      public int Id { get; set; }

      [Required]
      public string Company { get; set; }

      [Required]
      public string Position { get; set; }

      [Required]
      public string Content { get; set; }

      [Required]
      public string StartDate { get; set; }

      [Required]
      public string EndDate { get; set; }
   }
}
