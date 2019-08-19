using Blog.Domain;
using Blog.Domain.DeveloperStory;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Domain.DeveloperStory
{
   public class DeveloperTests
   {
      [Fact]
      public void Add_a_new_experience()
      {
         var developer = new Developer("summary", "skills");
         developer.AddExperience("Parmis",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.Experiences
            .Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(new
            {
               Company = "Parmis",
               Position = "C# Developer",
               StartDate = new DateTime(2015, 1, 1),
               EndDate = new DateTime(2016, 1, 1),
               Content = "done tasks"
            });
      }

      [Fact]
      public void Do_not_add_experience_with_duplicate_company_and_position()
      {
         var developer = new Developer("passionate developer", "c#, js");
         developer.AddExperience("Parmis",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.Invoking(d => d.AddExperience("Parmis",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks"))
            .Should()
            .Throw<DomainProblemException>()
            .Where(x => x.Problem.Property == string.Empty)
            .WithMessage("An experience of C# Developer at Parmis already exists");
      }

      [Fact]
      public void Add_multiple_experiences_for_the_same_company_with_different_positions()
      {
         var developer = new Developer("passionate developer", "C#, js");

         developer.AddExperience("Parmis",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.AddExperience("Parmis",
            "JS Developer",
            new DateTime(2016, 1, 2),
            new DateTime(2017, 1, 1),
            "done tasks");

         developer.Experiences.Should().HaveCount(2);
      }

      [Fact]
      public void Can_not_experiences_with_time_overlaps()
      {
         var developer = new Developer("passionate developer", "C#, JS");

         developer.AddExperience("Parmis",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.Invoking(d => d.AddExperience("Lodgify",
            "C# Developer",
            new DateTime(2015, 6, 1),
            new DateTime(2017, 1, 1),
            "done tasks"))
            .Should()
            .Throw<DomainProblemException>()
            .WithMessage("Experiences cannot have time overlaps with eachothers");
      }

      [Fact]
      public void Can_not_add_experiences_with_startDates_that_are_greater_than_endDates()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.Invoking(d => d.AddExperience("Lodgify",
            "C# Developer",
            new DateTime(2017, 1, 1),
            new DateTime(2015, 1, 1),
            "done tasks"))
            .Should()
            .Throw<DomainProblemException>()
            .Where(x => x.Problem.Property == "StartDate")
            .WithMessage("StartDate should be smaller than EndDate");
      }

      [Fact]
      public void Can_update_an_experience()
      {
         var developer = new Developer("passionate developer", "C#");

         developer.AddExperience("Lodgify",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.UpdateExperience(0,
            "Parmis",
            "JS Developer",
            new DateTime(2017, 1, 1),
            new DateTime(2018, 1, 1),
            "done tasks");

         developer.Experiences
            .Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(new
            {
               Company = "Parmis",
               Position = "JS Developer",
               StartDate = new DateTime(2017, 1, 1),
               EndDate = new DateTime(2018, 1, 1),
               Content = "done tasks"
            });
      }

      [Fact]
      public void Remove_experience()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.AddExperience("Lodgify",
            "C# Developer",
            new DateTime(2015, 1, 1),
            new DateTime(2016, 1, 1),
            "done tasks");

         developer.RemoveExperience(developer.Experiences.First());

         developer.Experiences
            .Should()
            .HaveCount(0);
      }

      [Fact]
      public void Can_not_set_summary_and_skills_to_empty_values()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.Invoking(d => d.Update("Depressed Developer", ""))
            .Should()
            .Throw<DomainProblemException>();
      }
   }
}
