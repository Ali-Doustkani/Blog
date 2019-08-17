using System;

namespace Blog.Domain.DeveloperStory
{
   public class Experience : DomainEntity
   {
      public Experience(string company, string position, DateTime startDate, DateTime endDate, string content)
      {
         if (startDate >= endDate)
            throw new DomainProblemException(nameof(StartDate), "StartDate should be smaller than EndDate");

         Company = Its.NotEmpty(company, nameof(Company));
         Position = Its.NotEmpty(position, nameof(Position));
         StartDate = startDate;
         EndDate = endDate;
         Content = Its.NotEmpty(content, nameof(Content));
      }

      public string Company { get; private set; }
      public string Position { get; private set; }
      public DateTime StartDate { get; private set; }
      public DateTime EndDate { get; private set; }
      public string Content { get; private set; }
   }
}
