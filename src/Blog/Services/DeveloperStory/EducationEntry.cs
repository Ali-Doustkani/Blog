using System.ComponentModel.DataAnnotations;

namespace Blog.Services.DeveloperStory
{
   public class EducationEntry
   {
      [Required]
      public string Id { get; set; }

      [Required]
      public string Degree { get; set; }

      [Required]
      public string University { get; set; }

      [Required]
      public string StartDate { get; set; }

      [Required]
      public string EndDate { get; set; }
   }
}
