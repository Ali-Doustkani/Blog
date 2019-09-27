using AutoMapper;
using Blog.Domain.DeveloperStory;
using Blog.Services.DeveloperSaveCommand;
using Blog.Services.DeveloperSaveQuery;
using FluentAssertions;
using MediatR;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Handler = Blog.Services.DeveloperSaveQuery.Handler;

namespace Blog.Tests.Services
{
   public class DeveloperSaveQueryTests
   {
      public DeveloperSaveQueryTests(ITestOutputHelper output)
      {
         _context = new TestContext(output);
         var db = _context.GetDb();
         var mapper = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>()).CreateMapper();
         _handler = new Handler(db, mapper);
      }

      private readonly TestContext _context;
      private readonly IRequestHandler<DeveloperSaveQuery, DeveloperSaveCommand> _handler;

      [Fact]
      public async Task Return_developer_when_its_available()
      {
         var command = new DeveloperUpdateCommand
         {
            Summary = "The best developer ever!",
            Skills = "C#, Javascript, React",
            Experiences = new[]
            {
               new ExperienceEntry{ Company="Parmis", Position="C# Developer", StartDate="2016-1-1", EndDate="2017-1-1", Content=  "System Architect" }
            },
            SideProjects = new[]
            {
               new SideProjectEntry{Title="Richtext Editor", Content="A simple richtext for web"}
            },
            Educations = new[]
            {
               new EducationEntry{Degree="BS",University="S&C", StartDate= "2010-1-1", EndDate="2011-1-1" }
            }
         };
         var developer = Developer.Create(command).Developer;

         using (var db = _context.GetDb())
         {
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DeveloperSaveQuery(), default);
         result.Should().BeEquivalentTo(new
         {
            Summary = "The best developer ever!",
            Skills = "C#, Javascript, React",
            Experiences = new[]
            {
               new
               {
                  Company = "Parmis",
                  Position = "C# Developer",
                  Content = "System Architect",
                  StartDate ="2016-01-01",
                  EndDate ="2017-01-01"
               }
            },
            SideProjects = new[]
            {
               new
               {
                  Title ="Richtext Editor",
                  Content ="A simple richtext for web"
               },
            },
            Educations = new[]
            {
               new
               {
                  Degree = "BS",
                  University = "S&C",
                  StartDate = "2010-01-01",
                  EndDate = "2011-01-01"
               }
            }
         });
      }

      [Fact]
      public async Task Sort_educations_by_date()
      {
         var command = new DeveloperUpdateCommand
         {
            Summary = "The best developer ever!",
            Skills = "C#, Javascript, React",
            Experiences = new[]
            {
               new ExperienceEntry{ Company="Lodgify", Position="C# Developer", StartDate="2010-1-1", EndDate="2011-1-1", Content=  "System Architect" },
               new ExperienceEntry{ Company="Parmis", Position="C# Developer", StartDate="2016-1-1", EndDate="2017-1-1", Content=  "System Architect" },
            },
            Educations = new[]
            {
               new EducationEntry{Degree="MS",University="S&C", StartDate= "2005-1-1", EndDate="2006-1-1" },
               new EducationEntry{Degree="BS",University="S&C", StartDate= "2010-1-1", EndDate="2011-1-1" },
            }
         };
         var developer = Developer.Create(command).Developer;

         using (var db = _context.GetDb())
         {
            db.Developers.Add(developer);
            db.SaveChanges();
         }

         var result = await _handler.Handle(new DeveloperSaveQuery(), default);

         result.Experiences
            .Select(x => x.StartDate)
            .Should()
            .ContainInOrder("2016-01-01", "2010-01-01");

         result.Educations
            .Select(x => x.StartDate)
            .Should()
            .ContainInOrder("2010-01-01", "2005-01-01");
      }
   }
}
