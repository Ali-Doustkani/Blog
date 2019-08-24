using Blog.Domain.DeveloperStory;
using FluentAssertions;
using System;
using Xunit;

namespace Blog.Tests.Domain
{
   public class PeriodTests
   {
      [Theory]
      [InlineData("2010-1-1", "2011-1-1", "2010-6-1", "2011-6-1", true)]
      [InlineData("2010-6-1", "2011-6-1", "2010-1-1", "2011-1-1", true)]
      [InlineData("2010-1-1", "2011-1-1", "2010-1-1", "2011-1-1", true)]
      [InlineData("2010-1-1", "2011-1-1", "2010-6-1", "2011-1-1", true)]
      [InlineData("2010-1-1", "2011-1-1", "2010-1-1", "2010-6-1", true)]
      [InlineData("2010-6-1", "2011-1-1", "2010-1-1", "2011-1-1", true)]
      [InlineData("2010-1-1", "2010-6-1", "2010-1-1", "2011-1-1", true)]
      [InlineData("2010-1-1", "2011-1-1", "2012-1-1", "2013-1-1", false)]
      [InlineData("2012-1-1", "2013-1-1", "2010-1-1", "2011-1-1", false)]
      [InlineData("2011-1-1", "2012-1-1", "2010-1-1", "2011-1-1", false)]
      [InlineData("2010-1-1", "2011-1-1", "2011-1-1", "2012-1-1", false)]
      public void Overlaps(string startA, string endA, string startB, string endB, bool result)
      {
         var period = new Period(Convert.ToDateTime(startA), Convert.ToDateTime(endA));
         period.Overlaps(new Period(Convert.ToDateTime(startB), Convert.ToDateTime(endB)))
            .Should()
            .Be(result);
      }
   }
}
