using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.DeveloperStory
{
   public class EducationEntry : DomainObjectEntry
   {
      [Required]
      public string Degree { get; set; }

      [Required]
      public string University { get; set; }

      [Required]
      public string StartDate { get; set; }

      [Required]
      public string EndDate { get; set; }

      public DateTime GetStartDate() => DateTime.Parse(StartDate);

      public DateTime GetEndDate() => DateTime.Parse(EndDate);
   }
}
