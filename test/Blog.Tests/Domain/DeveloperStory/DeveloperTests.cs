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
               Period = new
               {
                  StartDate = new DateTime(2015, 1, 1),
                  EndDate = new DateTime(2016, 1, 1),
               },
               Content = new
               {
                  RawContent = "done tasks"
               }
            });
      }

      [Fact]
      public void Dont_add_experience_with_duplicate_company_and_position()
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
      public void Dont_experiences_with_time_overlaps()
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
      public void Dont_add_experiences_with_startDates_that_are_greater_than_endDates()
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
      public void Sort_experiences_by_date()
      {
         var developer = new Developer("passionate dev", "C#");
         developer.AddExperience("Parmis", "C# Developer", new DateTime(2016, 1, 1), new DateTime(2017, 1, 1), "desc");
         developer.AddExperience("Lodgify", "C# Developer", new DateTime(2013, 1, 1), new DateTime(2014, 1, 1), "desc");

         developer.Experiences
            .ElementAt(0)
            .Should()
            .BeEquivalentTo(new
            {
               Period = new
               {
                  StartDate = new DateTime(2016, 1, 1),
                  EndDate = new DateTime(2017, 1, 1)
               }
            });

         developer.Experiences
           .ElementAt(1)
           .Should()
           .BeEquivalentTo(new
           {
              Period = new
              {
                 StartDate = new DateTime(2013, 1, 1),
                 EndDate = new DateTime(2014, 1, 1)
              }
           });
      }



      [Fact]
      public void Add_a_new_sideProject()
      {
         var developer = new Developer("passionate dev", "C#");
         developer.AddSideProject("Richtext", "web editor");

         developer.SideProjects
            .Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(new
            {
               Title = "Richtext",
               Content = new
               {
                  RawContent = "web editor"
               }
            });
      }

      [Fact]
      public void Dont_add_side_projects_with_duplicate_titles()
      {
         var developer = new Developer("passionate dev", "C#");
         developer.AddSideProject("Richtext", "web editor");
         developer.Invoking(d => d.AddSideProject("Richtext", "web editor"))
            .Should()
            .Throw<DomainProblemException>()
            .WithMessage("The 'Richtext' project already exists");
      }

      [Fact]
      public void GetSkillLines()
      {
         var developer = new Developer("passionate developer", "C#\nJS");
         developer.GetSkillLines()
            .Should()
            .ContainInOrder("C#", "JS");
      }

      [Fact]
      public void Add_a_new_education()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
         developer.Educations
            .Should()
            .HaveCount(1)
            .And
            .ContainEquivalentOf(new
            {
               Degree = "BS",
               University = "S&C",
               Period = new
               {
                  StartDate = new DateTime(2010, 1, 1),
                  EndDate = new DateTime(2011, 1, 1)
               }
            });
      }

      [Fact]
      public void Dont_add_education_with_same_degree_and_university()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
         developer
            .Invoking(d => d.AddEducation("BS", "S&C", new DateTime(2015, 1, 1), new DateTime(2016, 1, 1)))
            .Should()
            .Throw<DomainProblemException>().WithMessage("Another education item with the same degree and university already exists");
      }

      [Fact]
      public void Dont_add_education_with_overlapping_dates()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
         developer
            .Invoking(d => d.AddEducation("MS", "Other", new DateTime(2010, 6, 1), new DateTime(2012, 1, 1)))
            .Should()
            .Throw<DomainProblemException>()
            .WithMessage("Education items should not have date overlaps with each other");
      }

      [Fact]
      public void Sort_educations_by_date()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
         developer.AddEducation("MS", "S&C", new DateTime(2012, 1, 1), new DateTime(2013, 1, 1));

         developer.Educations
            .ElementAt(0)
            .Should()
            .BeEquivalentTo(new
            {
               Period = new
               {
                  StartDate = new DateTime(2012, 1, 1),
                  EndDate = new DateTime(2013, 1, 1)
               }
            });

         developer.Educations
            .ElementAt(1)
            .Should()
            .BeEquivalentTo(new
            {
               Period = new
               {
                  StartDate = new DateTime(2010, 1, 1),
                  EndDate = new DateTime(2011, 1, 1)
               }
            });
      }
   }
}
