using System;
using System.ComponentModel.DataAnnotations;

namespace Blog.Domain.DeveloperStory
{
   public class ExperienceEntry : DomainObjectEntry
   {
      [Required]
      public string Company { get; set; }

      [Required]
      public string Position { get; set; }

      [Required]
      public string StartDate { get; set; }

      [Required]
      public string EndDate { get; set; }

      [Required]
      public string Content { get; set; }

      public DateTime GetStartDate() => DateTime.Parse(StartDate);

      public DateTime GetEndDate() => DateTime.Parse(EndDate);

      public Period GetPeriod() => new Period(GetStartDate(), GetEndDate());
   }
}
