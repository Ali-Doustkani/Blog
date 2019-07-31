using System;

namespace Blog.Domain.DeveloperStory
{
   public class Experience : DomainEntity
   {
      public string Company { get; set; }
      public string Position { get; set; }
      public DateTime StartDate { get; set; }
      public DateTime EndDate { get; set; }
      public string Content { get; set; }
   }
}
