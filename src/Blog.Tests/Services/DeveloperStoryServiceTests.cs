using AutoMapper;
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
         _factory = new ContextFactory();
         var mapperConfig = new MapperConfiguration(x => x.AddProfile<MappingProfile>());
         _service = new DeveloperServices(_factory.Create(), mapperConfig.CreateMapper());
      }

      private readonly DeveloperServices _service;
      private readonly ContextFactory _factory;

      [Fact]
      public void Return_null_when_there_is_no_developer() =>
         _service
         .Get()
         .Should()
         .BeNull();

      [Fact]
      public void Return_developer_when_its_available()
      {
         using (var context = _factory.Create())
         {
            var developer = new Developer
            {
               Summary = "The best developer ever!",
               Skills = "C#, Javascript, React"
            };
            developer.Experiences.Add(new Experience
            {
               Company = "Parmis",
               Position = "C# Developer",
               Content = "System Architect",
               StartDate = new DateTime(2016, 1, 20),
               EndDate = new DateTime(2017, 1, 1)
            });
            developer.SideProjects.Add(new SideProject
            {
               Title = "Richtext Editor",
               Content = "A simple richtext for web"
            });
            context.Developers.Add(developer);
            context.SaveChanges();
         }

         _service
            .Get()
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

      [Fact]
      public void Add_when_there_is_no_developer_available()
      {
         var result = _service.Save(new DeveloperEntry
         {
            Summary = "Cool guy!",
            Skills = "C#, Javascript, React",
            Experiences = new[]
            {
               new ExperienceEntry
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
               new SideProjectEntry
               {
                  Title = "Richtext Editor",
                  Content = "A simple richtext for web"
               }
            }
         });

         result
            .Should()
            .BeEquivalentTo(new
            {
               Status = Status.Created,
               Experiences = new[] { 1 },
               SideProjects = new[] { 1 }
            });

         _service
            .Get()
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

      [Fact]
      public void Update_when_there_is_already_a_developer_available()
      {
         using (var ctx = _factory.Create())
         {
            var developer = new Developer
            {
               Summary = "So Cool!",
               Skills = "ES7, Node.js"
            };
            developer.Experiences.Add(new Experience
            {
               Company = "Lodgify",
               Content = "as backend developer",
               StartDate = new DateTime(2016, 2, 23),
               EndDate = new DateTime(2017, 1, 2),
               Position = "C# Developer"
            });
            developer.SideProjects.Add(new SideProject
            {
               Title = "Richtext Editor",
               Content = "A simple richtext for web"
            });

            ctx.Developers.Add(developer);
            ctx.SaveChanges();
         }

         var result = _service.Save(new DeveloperEntry
         {
            Summary = "Not so cool",
            Skills = "ES7, Node.js",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = 1,
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
                  Id = 1,
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

         _service
            .Get()
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

      [Fact]
      public void Update_experiences()
      {
         using (var ctx = _factory.Create())
         {
            var developer = new Developer
            {
               Summary = "Not so cool",
               Skills = "ES7, Node.js",
            };
            developer.Experiences.Add(new Experience
            {
               Company = "Lodgify",
               Content = "as backend developer",
               StartDate = new DateTime(2016, 2, 23),
               EndDate = new DateTime(2017, 1, 2),
               Position = "C# Developer"
            });
            developer.Experiences.Add(new Experience
            {
               Company = "Parmis",
               Content = "as the team lead",
               StartDate = new DateTime(2014, 2, 23),
               EndDate = new DateTime(2015, 1, 2),
               Position = "Java Developer"
            });
            ctx.Developers.Add(developer);
            ctx.SaveChanges();
         }

         var result = _service.Save(new DeveloperEntry
         {
            Summary = "Not so cool",
            Skills = "ES7, Node.js",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = 2,
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

         _service
            .Get()
            .Should()
            .BeEquivalentTo(new DeveloperEntry
            {
               Summary = "Not so cool",
               Skills = "ES7, Node.js",
               Experiences = new[]
               {
                  new ExperienceEntry
                  {
                     Id = 2,
                     Company = "Parmis",
                     Content = "as the team lead",
                     StartDate = "2014-01-01",
                     EndDate = "2015-01-01",
                     Position = "Not Java Developer"
                  },
                  new ExperienceEntry
                  {
                     Id = 3,
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

      [Fact]
      public void Update_sideProjects()
      {
         using (var ctx = _factory.Create())
         {
            var developer = new Developer
            {
               Summary = "Cool guy!",
               Skills = "C#, SQL"
            };
            developer.SideProjects.Add(new SideProject
            {
               Title = "Richtext Editor",
               Content = "A simple web richtext"
            });
            developer.SideProjects.Add(new SideProject
            {
               Title = "CodePrac",
               Content = "A simple app for practice coding"
            });
            ctx.Developers.Add(developer);
            ctx.SaveChanges();
         }

         var result = _service.Save(new DeveloperEntry
         {
            Summary = "Cool guy!",
            Skills = "C#, SQL",
            SideProjects = new[]
            {
               new SideProjectEntry
               {
                  Id = 2,
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

         _service
            .Get()
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
                      Id = 2,
                      Title = "CodePrac V2",
                      Content = "other description"
                  },
                  new SideProjectEntry
                  {
                      Id = 3,
                      Title = "Blog",
                      Content = "A developers blog"
                  },
               }
            });
      }
   }
}
