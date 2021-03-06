﻿namespace Blog.Domain.DeveloperStory
{
   public class Education : DomainEntity
   {
      private Education() : base(0) { }

      public Education(int id, string degree, string university, Period period)
         : base(id)
      {
         Degree = Assert.NotNull(degree);
         University = Assert.NotNull(university);
         Period = Assert.NotNull(period);
      }

      public string Degree { get; private set; }
      public string University { get; private set; }
      public Period Period { get; private set; }
   }
}
