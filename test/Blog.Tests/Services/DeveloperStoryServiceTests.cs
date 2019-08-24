using Blog.Domain.DeveloperStory;
using Blog.Services.DeveloperStory;
using FluentAssertions;
using System;
using System.Linq;
using Xunit;

namespace Blog.Tests.Services
{
   [Trait("Category", "Integration")]
   public class DeveloperStoryServiceTests
   {
      public DeveloperStoryServiceTests()
      {
         _context = new ServiceTestContext<DeveloperServices>();
      }

      private readonly ServiceTestContext<DeveloperServices> _context;

      [Fact]
      public void Return_null_when_there_is_no_developer()
      {
         using (var svc = _context.GetService())
         {
            svc.Get()
               .Should()
               .BeNull();
         }
      }

      [Fact]
      public void Return_developer_when_its_available()
      {
         using (var db = _context.GetDatabase())
         {
            var developer = new Developer("The best developer ever!", "C#, Javascript, React");
            developer.AddExperience(
               "Parmis",
               "C# Developer",
               new DateTime(2016, 1, 20),
               new DateTime(2017, 1, 1),
               "System Architect");
            developer.AddSideProject("Richtext Editor", "A simple richtext for web");
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         using (var svc = _context.GetService())
         {
            svc.Get()
            .Should()
            .BeEquivalentTo(new
            {
               Summary = "The best developer ever!",
               Skills = "C#, Javascript, React",
               Experiences = new[]
               {
                  new
                  {
                     Company = "Parmis",
                     Position= "C# Developer",
                     Content = "System Architect",
                     StartDate="2016-01-20",
                     EndDate="2017-01-01"
                  }
               },
               SideProjects = new[]
               {
                  new
                  {
                     Title="Richtext Editor",
                     Content="A simple richtext for web"
                  }
               }
            });
         }
      }

      [Fact]
      public void Add_when_there_is_no_developer_available()
      {
         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Cool guy!",
               Skills = "C#, Javascript, React",
               Experiences = new[]
               {
                  new ExperienceEntry
                  {
                     Id = "abc-123-efg",
                     Company = "Microsoft",
                     Content = "as backend developer",
                     StartDate = "2016-02-23",
                     EndDate = "2017-01-02",
                     Position = "Lead Developer"
                  }
               },
               SideProjects = new[]
               {
                  new SideProjectEntry
                  {
                     Id = "abc-123-efg",
                     Title = "Richtext Editor",
                     Content = "A simple richtext for web"
                  }
               }
            });

            result.Should().BeEquivalentTo(new
            {
               Status = Status.Created,
               Experiences = new[] { 1 },
               SideProjects = new[] { 1 }
            });
         }

         using (var svc = _context.GetService())
         {
            svc.Get()
               .Should()
               .BeEquivalentTo(new
               {
                  Summary = "Cool guy!",
                  Skills = "C#, Javascript, React",
                  Experiences = new[]
                  {
                     new
                     {
                        Company = "Microsoft",
                        Content = "as backend developer",
                        StartDate = "2016-02-23",
                        EndDate = "2017-01-02",
                        Position = "Lead Developer"
                     }
                  },
                  SideProjects = new[]
                  {
                     new
                     {
                        Title = "Richtext Editor",
                        Content = "A simple richtext for web"
                     }
                  }
               });
         }
      }

      [Fact]
      public void Update_when_there_is_already_a_developer_available()
      {
         using (var db = _context.GetDatabase())
         {
            var developer = new Developer("So Cool!", "ES7, Node.js");
            developer.AddExperience(
               "Lodgify",
               "C# Developer",
               new DateTime(2016, 2, 23),
               new DateTime(2017, 1, 2),
               "as backend developer");
            developer.AddSideProject("Richtext Editor", "A simple richtext for web");

            db.Developers.Add(developer);
            db.SaveChanges();
         }

         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Not so cool",
               Skills = "ES7, Node.js",
               Experiences = new[]
               {
                  new ExperienceEntry
                  {
                     Id = "1",
                     Company = "Lodgify",
                     Content = "as backend developer",
                     StartDate = "2016-02-23",
                     EndDate = "2017-01-02",
                     Position = "C# Developer"
                  }
               },
               SideProjects = new[]
               {
                  new SideProjectEntry
                  {
                     Id = "1",
                     Title = "Richtext Editor",
                     Content = "A simple richtext for web"
                  }
               }
            });

            result
               .Should()
               .BeEquivalentTo(new
               {
                  Status = Status.Updated,
                  Experiences = new[] { 1 },
                  SideProjects = new[] { 1 }
               });
         }

         using (var svc = _context.GetService())
         {
            svc.Get()
               .Should()
               .BeEquivalentTo(new
               {
                  Summary = "Not so cool",
                  Skills = "ES7, Node.js",
                  Experiences = new[]
                  {
                     new
                     {
                        Company="Lodgify",
                        Content="as backend developer",
                        StartDate="2016-02-23",
                        EndDate="2017-01-02",
                        Position="C# Developer"
                     }
                  },
                  SideProjects = new[]
                  {
                     new
                     {
                        Title = "Richtext Editor",
                        Content = "A simple richtext for web"
                     }
                  }
               });
         }
      }

      [Fact]
      public void Update_experiences()
      {
         using (var db = _context.GetDatabase())
         {
            var developer = new Developer("Not so cool", "ES7, Node.js");
            developer.AddExperience(
               "Lodgify",
               "C# Developer",
               new DateTime(2016, 2, 23),
               new DateTime(2017, 1, 2),
               "as backend developer");
            developer.AddExperience(
               "Parmis",
               "Java Developer",
               new DateTime(2017, 2, 23),
               new DateTime(2018, 1, 2),
               "as the team lead");
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Not so cool",
               Skills = "ES7, Node.js",
               Experiences = new[]
               {
                  new ExperienceEntry
                  {
                     Id = "2",
                     Company = "Parmis",
                     Content = "as the team lead",
                     StartDate = "2014-01-01",
                     EndDate = "2015-01-01",
                     Position = "Not Java Developer"
                  },
                  new ExperienceEntry
                  {
                       Company = "Bellin",
                       Content = "agile c# developer",
                       StartDate = "2019-01-01",
                       EndDate = "2022-01-01",
                       Position = "Tester"
                  }
               }
            });

            result
               .Should()
               .BeEquivalentTo(new
               {
                  Experiences = new[] { 2, 3 },
                  SideProjects = Enumerable.Empty<int>()
               });
         }

         using (var svc = _context.GetService())
         {
            svc.Get()
               .Should()
               .BeEquivalentTo(new DeveloperEntry
               {
                  Summary = "Not so cool",
                  Skills = "ES7, Node.js",
                  Experiences = new[]
                  {
                     new ExperienceEntry
                     {
                        Id = "2",
                        Company = "Parmis",
                        Content = "as the team lead",
                        StartDate = "2014-01-01",
                        EndDate = "2015-01-01",
                        Position = "Not Java Developer"
                     },
                     new ExperienceEntry
                     {
                        Id = "3",
                        Company = "Bellin",
                        Content = "agile c# developer",
                        StartDate = "2019-01-01",
                        EndDate = "2022-01-01",
                        Position = "Tester"
                     }
                  },
                  SideProjects = Enumerable.Empty<SideProjectEntry>()
               });
         }
      }

      [Fact]
      public void Update_sideProjects()
      {
         using (var db = _context.GetDatabase())
         {
            var developer = new Developer("Cool guy!", "C#, SQL");
            developer.AddSideProject("Richtext Editor", "A simple web richtext");
            developer.AddSideProject("CodePrac", "A simple app for practice coding");
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Cool guy!",
               Skills = "C#, SQL",
               SideProjects = new[]
               {
                  new SideProjectEntry
                  {
                     Id = "2",
                     Title = "CodePrac V2",
                     Content = "other description"
                  },
                  new SideProjectEntry
                  {
                     Title = "Blog",
                     Content = "A developers blog"
                  }
               }
            });

            result
               .Should()
               .BeEquivalentTo(new
               {
                  Experiences = Enumerable.Empty<int>(),
                  SideProjects = new[] { 2, 3 }
               });
         }

         using (var svc = _context.GetService())
         {
            svc.Get()
               .Should()
               .BeEquivalentTo(new DeveloperEntry
               {
                  Summary = "Cool guy!",
                  Skills = "C#, SQL",
                  Experiences = Enumerable.Empty<ExperienceEntry>(),
                  SideProjects = new[]
                  {
                     new SideProjectEntry
                     {
                         Id = "2",
                         Title = "CodePrac V2",
                         Content = "other description"
                     },
                     new SideProjectEntry
                     {
                         Id = "3",
                         Title = "Blog",
                         Content = "A developers blog"
                     },
                  }
               });
         }
      }

      [Fact]
      public void Return_problem_when_developer_required_fields_are_empty()
      {
         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Skills is not set!"
            });

            result.Should()
               .BeEquivalentTo(new
               {
                  Status = Status.Problem,
                  Problem = new
                  {
                     Property = "Skills",
                     Message = "Value is required"
                  }
               });
         }
      }

      [Fact]
      public void Return_problem_when_experience_required_fields_are_empty()
      {
         using (var svc = _context.GetService())
         {
            var result = svc.Save(new DeveloperEntry
            {
               Summary = "Cool Developer!",
               Skills = "C#, JS",
               Experiences = new[]
               {
                  new ExperienceEntry
                  {
                     Position = "Senior Developer",
                     StartDate = "2011-01-01",
                     EndDate = "2012-01-01",
                     Content = "an enterprise project"
                  }
               }
            });

            result.Should()
               .BeEquivalentTo(new
               {
                  Status = Status.Problem,
                  Problem = new
                  {
                     Property = "Company",
                     Message = "Value is required"
                  }
               });
         }
      }
   }
}
