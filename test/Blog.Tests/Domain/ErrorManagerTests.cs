using Blog.Domain;
using FluentAssertions;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain
{
   public class ErrorManagerTests
   {
      public class Person
      {
         public string Name { get; set; }
         public string Surname { get; set; }
         public int Age { get; set; }
         public string Start { get; set; }
         public string End { get; set; }
      }

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

      [Fact]
      public void NoDuplicate_with_single_property_check()
      {
         var values = new[]
         {
            new Person { Name = "Ali"},
            new Person { Name = "Haniye"},
            new Person { Name = "Ali" },
            new Person { Name = "Haniye" }
         };

         new ErrorManager()
            .NoDuplicate(values,
               x => x.Name,
               person => $"'{person.Name}' is a duplicate")
            .Errors
            .Should()
            .ContainInOrder(
               "'Ali' is a duplicate",
               "'Haniye' is a duplicate"
            );
      }

      [Fact]
      public void NoDuplicate_with_two_properties_check()
      {
         var values = new[]
         {
            new Person { Name="Ali", Surname="Doustkani"},
            new Person { Name="Ali", Surname="Moradi"},
            new Person { Name="Haniye", Surname= "Doustkani"},
            new Person { Name="Ali", Surname="Doustkani"}
         };

         new ErrorManager()
            .NoDuplicate(values,
               x => new { x.Name, x.Surname },
               person => $"'{person.Name} {person.Surname}' is a duplicate")
            .Errors
            .Should()
            .ContainSingle("'Ali Doustkani' is a duplicate");
      }

      [Fact]
      public void NoDuplicate_with_string_and_int()
      {
         var values = new[]
       {
            new Person { Name="Ali", Age=12},
            new Person { Name="Ali", Age=13 },
            new Person { Name="Haniye", Age=12},
            new Person { Name="Ali", Age=12}
         };

         new ErrorManager()
            .NoDuplicate(values,
               x => new { x.Name, x.Age },
               person => $"'{person.Name}' with age of '{person.Age}' is duplicated")
            .Errors
            .Should()
            .ContainSingle("'Ali' with age of '12' is duplicated");
      }

      [Fact]
      public void NoOverlaps_with_two_identicals()
      {
         var values = new[]
         {
            new Person { Name="A", Start="2010-01-01", End="2011-01-01" },
            new Person { Name="B", Start="2011-01-01", End="2012-01-01" },
            new Person { Name="C", Start="2010-06-01", End="2011-01-01" },
         };
         new ErrorManager()
            .NoOverlaps(values,
               x => x.Start,
               x => x.End,
               (person, others) => $"{person.Name} overlaps with {others.ElementAt(0).Name}")
            .Errors
            .Should()
            .ContainSingle("A overlaps with C");
      }

      [Fact]
      public void NoOverlaps_with_more_than_two_identicals()
      {
         var values = new[]
         {
            new Person { Name="A", Start="2010-01-01", End="2011-01-01" },
            new Person { Name="B", Start="2011-01-01", End="2012-01-01" },
            new Person { Name="C", Start="2010-06-01", End="2011-01-01" },
            new Person { Name="D", Start="2010-06-01", End="2012-01-01" },
         };
         new ErrorManager()
            .NoOverlaps(values,
               x => x.Start,
               x => x.End,
               (person, others) => $"{person.Name} overlaps with {string.Join(',', others.Select(x => x.Name))}")
            .Errors
            .Should()
            .ContainInOrder(
            "A overlaps with C,D",
            "B overlaps with D");
      }

      [Fact]
      public void CheckPeriods()
      {
         var values = new[]
         {
            new Person { Name="A", Start="2010-01-01", End="2011-01-01" },
            new Person { Name="B", Start="2013-01-01", End="2012-01-01" }
         };
         new ErrorManager()
            .CheckPeriods(values,
               x => x.Start,
               x => x.End,
               person => $"{person.Name} StartDate is greater than EndDate")
            .Errors
            .Should()
            .ContainSingle("B StartDate is greater than EndDate");
      }

      [Fact]
      public void Conditional_errors_with_single_rule()
      {
         new ErrorManager()
            .Conditional(
               pre => pre.IfTrue(true, "A"),
               post => post.IfTrue(true, "B"))
            .Errors
            .Should()
            .ContainInOrder("A");

         new ErrorManager()
           .Conditional(
              pre => pre.IfTrue(false, "A"),
              post => post.IfTrue(true, "B"))
           .Errors
           .Should()
           .ContainInOrder("B");
      }

      [Fact]
      public void Conditional_errors_with_multiple_rules()
      {
         new ErrorManager()
            .Conditional(
               pre => pre.IfTrue(false, "A").IfTrue(true, "B"),
               post => post.IfTrue(true, "C"))
            .Errors
            .Should()
            .ContainInOrder("B");

         new ErrorManager()
           .Conditional(
              pre => pre.IfTrue(false, "A").IfTrue(false, "B"),
              post => post.IfTrue(true, "C"))
           .Errors
           .Should()
           .ContainInOrder("C");
      }

      [Fact]
      public void Merge_conditional_errors_with_its_parent()
      {
         new ErrorManager()
            .IfTrue(true, "A")
            .Conditional(
               cond => cond.IfTrue(true, "B"),
               then => then.IfTrue(true, "C"))
            .IfTrue(true, "D")
            .Errors
            .Should()
            .ContainInOrder("A", "B", "D");
      }
   }
}
