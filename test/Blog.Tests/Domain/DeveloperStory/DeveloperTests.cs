using Blog.Domain;
using Blog.Domain.DeveloperStory;
using FluentAssertions;
using Moq;
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
         developer.Update(new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate = "2015-1-1",
                  EndDate = "2016-1-1",
                  Content = "done tasks"
               }
            }
         }, Mock.Of<IStorageState>());


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
         var command = new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate = "2015-1-1",
                  EndDate = "2016-1-1",
                  Content = "done tasks"
               },
               new ExperienceEntry
               {
                  Id = "b",
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate = "2015-1-1",
                  EndDate = "2016-1-1",
                  Content = "done tasks"
               }
            }
         };

         developer.Invoking(d => d.Update(command, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
      }

      [Fact]
      public void Add_multiple_experiences_for_the_same_company_with_different_positions()
      {
         var developer = new Developer("passionate developer", "C#, js");
         developer.Update(new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate ="2015-1-1",
                  EndDate = "2016-1-1",
                  Content ="done tasks"
               },
               new ExperienceEntry
               {
                  Company ="Parmis",
                  Position ="JS Developer",
                  StartDate ="2016-1-2",
                  EndDate ="2017-1-1",
                  Content ="done tasks"
               }
            }
         }, Mock.Of<IStorageState>());

         developer.Experiences.Should().HaveCount(2);
      }

      [Fact]
      public void Dont_update_experiences_with_time_overlaps()
      {
         var developer = new Developer("passionate developer", "C#, JS");
         var command = new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate = "2015-1-1",
                  EndDate = "2016-1-1",
                  Content = "done tasks"
               },
               new ExperienceEntry
               {
                  Id = "b",
                  Company = "Lodgify",
                  Position = "C# Developer",
                  StartDate = "2015-6-1",
                  EndDate = "2017-1-1",
                  Content = "done tasks"
               }
            }
         };

         developer.Invoking(d => d.Update(command, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
      }

      [Fact]
      public void Dont_add_experiences_with_startDates_that_are_greater_than_endDates()
      {
         var developer = new Developer("passionate developer", "C#");
         var add = new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Company="Lodgify",
                  Position = "C# Developer",
                  StartDate = "2017-1-1",
                  EndDate = "2015-1-1",
                  Content = "done tasks"
               }
            }
         };

         developer.Invoking(d => d.Update(add, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
      }

      [Fact]
      public void Sort_experiences_by_date()
      {
         var developer = new Developer("passionate dev", "C#");
         developer.Update(new DeveloperUpdateCommand
         {
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Company = "Parmis",
                  Position = "C# Developer",
                  StartDate = "2016-1-1",
                  EndDate = "2017-1-1",
                  Content = "desc"
               },
               new ExperienceEntry
               {
                  Company = "Lodgify",
                  Position = "C# Developer",
                  StartDate = "2013-1-1",
                  EndDate = "2014-1-1",
                  Content = "desc"
               }
            }
         }, Mock.Of<IStorageState>());

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
         developer.Update(new DeveloperUpdateCommand
         {
            SideProjects = new[]
            {
               new SideProjectEntry
               {
                  Title = "Richtext",
                  Content = "web editor"
               }
            }
         }, Mock.Of<IStorageState>());

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
         var add = new DeveloperUpdateCommand
         {
            SideProjects = new[]
            {
               new SideProjectEntry
               {
                  Id = "a",
                  Title = "Richtext",
                  Content = "web editor"
               },
               new SideProjectEntry
               {
                  Id = "b",
                  Title = "Richtext",
                  Content = "web editor"
               }
            }
         };
         developer.Invoking(d => d.Update(add, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
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
         developer.Update(new DeveloperUpdateCommand
         {
            Educations = new[]
            {
               new EducationEntry
               {
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               }
            }
         }, Mock.Of<IStorageState>());

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
         var add = new DeveloperUpdateCommand
         {
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "a",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               },
               new EducationEntry
               {
                  Id = "b",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2015-1-1",
                  EndDate = "2016-1-1"
               }
            }
         };
         developer.Invoking(d => d.Update(add, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
      }

      [Fact]
      public void Dont_add_education_with_overlapping_dates()
      {
         var developer = new Developer("passionate developer", "C#");
         var add = new DeveloperUpdateCommand
         {
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "a",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               },
               new EducationEntry
               {
                  Id = "b",
                  Degree = "MS",
                  University = "Other",
                  StartDate = "2010-6-1"                  ,
                  EndDate ="2012-1-1"
               }
            }
         };
         developer.Invoking(d => d.Update(add, Mock.Of<IStorageState>()))
            .Should()
            .Throw<ArgumentException>();
      }

      [Fact]
      public void Sort_educations_by_date()
      {
         var developer = new Developer("passionate developer", "C#");
         developer.Update(new DeveloperUpdateCommand
         {
            Educations = new[]
            {
               new EducationEntry
               {
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               },
               new EducationEntry
               {
                  Degree = "MS",
                  University = "S&C",
                  StartDate = "2012-1-1",
                  EndDate = "2013-1-1"
               }
            }
         }, Mock.Of<IStorageState>());

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
