using System;

namespace Blog.Domain.DeveloperStory
{
   public class Period
   {
      public Period(DateTime startDate, DateTime endDate)
      {
         if (startDate >= endDate)
            throw new ArgumentException(nameof(StartDate), "StartDate should be smaller than EndDate");

         StartDate = startDate;
         EndDate = endDate;
      }

      public DateTime StartDate { get; private set; }
      public DateTime EndDate { get; private set; }

      public bool Overlaps(Period other)
      {
         if (other == null)
            throw new ArgumentNullException(nameof(other));

         return StartDate < other.EndDate && EndDate > other.StartDate;
      }

      public override bool Equals(object obj)
      {
         var other = obj as Period;
         if (other == null)
            return false;

         return other.StartDate == StartDate && other.EndDate == EndDate;
      }

      public override int GetHashCode()
      {
         return StartDate.GetHashCode() ^ EndDate.GetHashCode();
      }
   }
}
