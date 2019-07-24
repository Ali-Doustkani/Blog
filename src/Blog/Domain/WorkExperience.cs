using System;

namespace Blog.Domain
{
   public class WorkExperience : DomainEntity
   {
      public string Title { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public string Content { get; set; }
   }
}
