using AutoMapper;
using Blog.Services.DeveloperSaveCommand;
using Blog.Services.DeveloperSaveQuery;
using Blog.Domain.DeveloperStory;
using FluentAssertions;
using MediatR;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Handler = Blog.Services.DeveloperSaveQuery.Handler;

namespace Blog.Tests.CQ
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
         var experiences = new[]
         {
            new Experience(0,
            "Parmis",
            "C# Developer",
            new Period(new DateTime (2016,1,1), new DateTime (2017,1,1)),
            "System Architect"   )
         };
         var developer = new Developer("The best developer ever!", "C#, Javascript, React", experiences);
         developer.AddSideProject("Richtext Editor", "A simple richtext for web");
         developer.AddEducation("BS", "S&C", new DateTime(2010, 1, 1), new DateTime(2011, 1, 1));
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
   }
}
