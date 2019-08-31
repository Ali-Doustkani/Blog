﻿using Blog.Domain;
using FluentAssertions;
using Xunit;

namespace Blog.Tests.Domain
{
   public class ErrorManagerTests
   {
      [Fact]
      public void CheckEmpty() =>
          new ErrorManager()
         .Required("", "A")
         .Required(" ", "B")
         .Required("Ali", "C")
         .Errors
         .Should()
         .BeEquivalentTo(new[]
         {
            "'A' is required" ,
            "'B' is required" ,
         });

      [Fact]
      public void IfTrue() =>
         new ErrorManager()
        .IfTrue(true, "A")
        .IfTrue(false, "B")
        .Errors
        .Should()
        .BeEquivalentTo(new[] { "A" });
   }
}
