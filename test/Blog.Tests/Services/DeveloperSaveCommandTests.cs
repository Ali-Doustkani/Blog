using AutoMapper;
using Blog.Domain.DeveloperStory;
using Blog.Services.DeveloperSaveCommand;
using FluentAssertions;
using MediatR;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Blog.Tests.CQ
{
   public class DeveloperSaveCommandTests
   {
      public DeveloperSaveCommandTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(_context.GetDb(), mapper);
      }

      private readonly TestContext _context;
      private readonly IRequestHandler<DeveloperSaveCommand, DeveloperSaveResult> _handler;

      [Fact]
      public async Task Add_when_there_is_no_developer_available()
      {
         var add1 = new DeveloperSaveCommand
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
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "frg-234",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               }
            }
         };

         var result = await _handler.Handle(add1, default);
         result.Created.Should().BeTrue();
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = new[] { 1 },
            Educations = new[] { 1 }
         });

         var add2 = new DeveloperSaveCommand
         {
            Summary = "Cool guy!",
            Skills = "C#, Javascript, React",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
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
                  Id = "a",
                  Title = "Richtext Editor",
                  Content = "A simple richtext for web"
               }
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "a",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-01-01",
                  EndDate = "2011-01-01"
               }
            }
         };

         result = await _handler.Handle(add2, default);
         result.Created.Should().BeFalse();
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 2 },
            SideProjects = new[] { 2 },
            Educations = new[] { 2 }
         });
      }

      [Fact]
      public async Task Update_when_there_is_already_a_developer_available()
      {
         var add = new DeveloperSaveCommand
         {
            Summary = "So Cool!",
            Skills = "ES7, Node.js",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "Lodgify",
                  Position = "C# Developer",
                  StartDate = "2016-2-23",
                  EndDate = "2017-1-2",
                  Content = "as backend developer"
               }
            },
            SideProjects = new[]
            {
               new SideProjectEntry
               {
                  Id = "a",
                  Title = "Richtext Editor",
                  Content = "A simple richtext for web"
               }
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "a",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               }
            }
         };

         await _handler.Handle(add, default);

         var update = new DeveloperSaveCommand
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
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "1",
                  Degree = "B.S.",
                  University = "Science and Culture",
                  StartDate = "2009-01-01",
                  EndDate = "2010-01-01"
               }
            }
         };

         var result = await _handler.Handle(update, default);
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = new[] { 1 },
            Educations = new[] { 1 }
         });

         using (var db = _context.GetDb())
         {
            db.GetDeveloper().Should().BeEquivalentTo(new
            {
               Summary = new
               {
                  RawContent = "Not so cool"
               },
               Skills = "ES7, Node.js",
               Experiences = new[]
               {
                  new
                  {
                     Id = 1,
                     Company ="Lodgify",
                     Content = new
                     {
                        RawContent = "as backend developer"
                     },
                     Period = new
                     {
                        StartDate = new  DateTime(2016,2,23),
                        EndDate =new DateTime(2017,1,2),
                     },
                     Position ="C# Developer"
                  }
               },
               SideProjects = new[]
               {
                  new
                  {
                     Id = 1,
                     Title = "Richtext Editor",
                     Content = new
                     {
                        RawContent = "A simple richtext for web"
                     }
                  }
               },
               Educations = new[]
               {
                  new
                  {
                     Id = 1,
                     Degree = "B.S.",
                     University = "Science and Culture",
                     Period = new
                     {
                        StartDate = new DateTime(2009,1,1),
                        EndDate = new DateTime(2010,1,1)
                     }
                  }
               }
            });
         }
      }

      [Fact]
      public async Task Update_experiences()
      {
         var add = new DeveloperSaveCommand
         {
            Summary = "Not so cool",
            Skills = "ES7, Node.js",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "Lodgify",
                  Position = "C# Developer",
                  StartDate = "2016-2-23",
                  EndDate = "2017-1-2",
                  Content = "as backend developer"
               },
               new ExperienceEntry
               {
                  Id = "b",
                  Company = "Parmis",
                  Position = "Java Developer",
                  StartDate = "2017-2-23",
                  EndDate = "2018-1-2",
                  Content = "as the team lead"
               }
            }
         };
         await _handler.Handle(add, default);

         var update = new DeveloperSaveCommand
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
                  Id = "ddd",
                  Company = "Bellin",
                  Content = "agile c# developer",
                  StartDate = "2019-01-01",
                  EndDate = "2022-01-01",
                  Position = "Tester"
               }
            }
         };

         var result = await _handler.Handle(update, default);
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 2, 3 },
            SideProjects = Enumerable.Empty<int>(),
            Educations = Enumerable.Empty<int>()
         });

         using (var db = _context.GetDb())
         {
            db.GetDeveloper().Should().BeEquivalentTo(new
            {
               Summary = new
               {
                  RawContent = "Not so cool"
               },
               Skills = "ES7, Node.js",
               Experiences = new[]
               {
                  new
                  {
                     Id = 3,
                     Company = "Bellin",
                     Position = "Tester",
                     Period = new
                     {
                        StartDate = new DateTime(2019,1,1),
                        EndDate = new DateTime(2022,1,1),
                     },
                     Content = new
                     {
                        RawContent = "agile c# developer"
                     }
                  },
                  new
                  {
                     Id = 2,
                     Company = "Parmis",
                     Position = "Not Java Developer",
                     Period = new
                     {
                        StartDate = new DateTime(2014,1,1),
                        EndDate = new DateTime(2015,1,1),
                     },
                     Content = new
                     {
                        RawContent = "as the team lead"
                     }
                  }
               },
               SideProjects = Enumerable.Empty<SideProjectEntry>(),
               Educations = Enumerable.Empty<EducationEntry>()
            });
         }
      }

      [Fact]
      public async Task Update_sideProjects()
      {
         var add = new DeveloperSaveCommand
         {
            Summary = "Cool guy!",
            Skills = "C#, SQL",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "parmis",
                  Position = "C# Developer",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1",
                  Content = "web development"
               }
            },
            SideProjects = new[]
            {
               new SideProjectEntry
               {
                  Id = "a",
                  Title = "Richtext Editor",
                  Content = "A simple web richtext"
               },
               new SideProjectEntry
               {
                  Id = "b",
                  Title = "CodePrac",
                  Content = "A simple app for practice coding"
               }
            }
         };

         await _handler.Handle(add, default);

         var update = new DeveloperSaveCommand
         {
            Summary = "Cool guy!",
            Skills = "C#, SQL",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "1",
                  Company = "parmis",
                  Position = "C# Developer",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1",
                  Content = "web development"
               }
            },
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
                  Id = "a",
                  Title = "Blog",
                  Content = "A developers blog"
               }
            }
         };

         var result = await _handler.Handle(update, default);
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = new[] { 2, 3 },
            Educations = Enumerable.Empty<int>()
         });

         using (var db = _context.GetDb())
         {
            db.GetDeveloper().Should().BeEquivalentTo(new
            {
               Summary = new
               {
                  RawContent = "Cool guy!"
               },
               Skills = "C#, SQL",
               Experiences = new[]
               {
                  new
                  {
                     Id = 1,
                     Company = "parmis",
                     Position = "C# Developer",
                     Period = new
                     {
                        StartDate = new DateTime(2010,1,1),
                        EndDate =  new DateTime(2011,1,1),
                     },
                     Content = new
                     {
                        RawContent = "web development"
                     }
                  }
               },
               Educations = Enumerable.Empty<EducationEntry>(),
               SideProjects = new[]
               {
                  new
                  {
                     Id = 2,
                     Title = "CodePrac V2",
                     Content = new
                     {
                        RawContent = "other description"
                     }
                  },
                  new
                  {
                     Id = 3,
                     Title = "Blog",
                     Content = new
                     {
                        RawContent = "A developers blog"
                     }
                  },
               }
            });
         }
      }

      [Fact]
      public async Task Update_educations()
      {
         var add = new DeveloperSaveCommand
         {
            Summary = "Cool guy!",
            Skills = "C#, SQL",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "a",
                  Company = "parmis",
                  Position = "C# Developer",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1",
                  Content = "web development"
               }
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "a",
                  Degree = "AS",
                  University = "College1",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1"
               },
               new EducationEntry
               {
                  Id = "b",
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2011-1-1",
                  EndDate = "2012-1-1"
               }
            }
         };

         await _handler.Handle(add, default);

         var update = new DeveloperSaveCommand
         {
            Summary = "Cool guy!",
            Skills = "C#, SQL",
            Experiences = new[]
            {
               new ExperienceEntry
               {
                  Id = "1",
                  Company = "parmis",
                  Position = "C# Developer",
                  StartDate = "2010-1-1",
                  EndDate = "2011-1-1",
                  Content = "web development"
               }
            },
            Educations = new[]
            {
               new EducationEntry
               {
                  Id = "2",
                  Degree = "B.S.",
                  University = "Science & Culture",
                  StartDate = "2015-01-01",
                  EndDate = "2016-01-01"
               },
               new EducationEntry
               {
                  Id = "a",
                  Degree = "M.S.",
                  University = "Tehran",
                  StartDate = "2017-01-01",
                  EndDate = "2018-01-01"
               }
            }
         };

         var result = await _handler.Handle(update, default);
         result.UpdateResult.Should().BeEquivalentTo(new
         {
            Experiences = new[] { 1 },
            SideProjects = Enumerable.Empty<int>(),
            Educations = new[] { 2, 3 }
         });

         using (var db = _context.GetDb())
         {
            db.GetDeveloper().Should().BeEquivalentTo(new
            {
               Summary = new
               {
                  RawContent = "Cool guy!"
               },
               Skills = "C#, SQL",
               Experiences = new[]
               {
                  new
                  {
                     Id = 1,
                     Company = "parmis",
                     Position = "C# Developer",
                     Period = new
                     {
                        StartDate = new DateTime(2010,1,1),
                        EndDate = new DateTime (2011,1,1),
                     },
                     Content = new
                     {
                        RawContent = "web development"
                     }
                  }
               },
               SideProjects = Enumerable.Empty<SideProjectEntry>(),
               Educations = new[]
               {
                  new
                  {
                     Id = 3,
                     Degree = "M.S.",
                     University = "Tehran",
                     Period = new
                     {
                        StartDate = new DateTime(2017,1,1),
                        EndDate = new DateTime(2018,1,1)
                     }
                  },
                  new
                  {
                     Id = 2,
                     Degree = "B.S.",
                     University = "Science & Culture",
                     Period = new
                     {
                        StartDate = new DateTime(2015,1,1),
                        EndDate = new DateTime(2016,1,1)
                     }
                  }
               }
            });
         }
      }
   }
}
