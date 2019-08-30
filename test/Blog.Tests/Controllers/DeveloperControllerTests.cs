using Blog.Domain.DeveloperStory;
using FluentAssertions;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Controllers
{
   public class DeveloperControllerTests : IClassFixture<ApiTestContext>
   {
      public DeveloperControllerTests(ApiTestContext fixture)
      {
         fixture.InitializeDatabase();
         _client = fixture.Client;
      }

      private readonly HttpClient _client;

      [Fact]
      public async Task Return_204_when_no_developer_is_available()
      {
         using (var response = await _client.GetAsync("/api/developer"))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.NoContent);
         }
      }

      [Fact]
      public async Task Return_201_when_new_developer_is_added()
      {
         var developer = new
         {
            summary = "a web programmer",
            skills = "C#, HTML",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "parmis",
                  position = "C# Developer",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1",
                  content = "web development"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.Created);
         }
      }

      [Fact]
      public async Task Return_200_when_developer_is_updated()
      {
         await _client.PutAsJsonAsync("/api/developer", new
         {
            summary = "a web programmer",
            skills = "C#, HTML",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "parmis",
                  position = "C# Developer",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1",
                  content = "web development"
               }
            }
         });

         var update = new
         {
            summary = "a passionate web developer",
            skills = "C#",
            experiences = new[]
            {
               new
               {
                  id = "1",
                  company = "parmis",
                  position = "C# Developer",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1",
                  content = "web development"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", update))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.OK);
         }
      }

      [Fact]
      public async Task Return_developer_when_its_available()
      {
         var developer = new
         {
            summary = "The best developer ever!",
            skills = "C#, Javascript, React",
            experiences = new[]
            {
               new
               {
                  id = "a",
                  company = "Parmis",
                  position = "C# Developer",
                  startDate = "2016-1-1",
                  endDate = "2017-1-1",
                  content = "System Architect"
               }
            },
            sideProjects = new[]
            {
               new
               {
                  id = "a",
                  title = "Richtext Editor",
                  content = "A simple richtext for web"
               }
            },
            educations = new[]
            {
               new
               {
                  id = "a",
                  degree = "BS",
                  university = "S&C",
                  startDate = "2010-1-1",
                  endDate = "2011-1-1"
               }
            }
         };

         await _client.PutAsJsonAsync("/api/developer", developer);

         using (var response = await _client.GetAsync("/api/developer"))
         {
            var result = JsonConvert.DeserializeObject<DeveloperUpdateCommand>(await response.Content.ReadAsStringAsync());
            result.Should().BeEquivalentTo(new
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
                     StartDate="2016-01-01",
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

      [Fact]
      public async Task Validate_input()
      {
         var developer = new
         {
            experiences = new[]
            {
               new { Id = "abc-123" }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());

            var dic = new Dictionary<string, object>
            {
               { "Skills", new[] {"'Skills' must not be empty."}},
               { "Summary", new[] {"'Summary' must not be empty."}},
               { "Experiences[0].Company", new[] {"'Company' must not be empty."}},
               { "Experiences[0].Content", new[] {"'Content' must not be empty."}},
               { "Experiences[0].EndDate", new[] {"Date string is invalid"}},
               { "Experiences[0].Position", new[] {"'Position' must not be empty."}},
               { "Experiences[0].StartDate", new[] {"Date string is invalid"}},
            };

            result.Should().BeEquivalentTo(JsonConvert.SerializeObject(dic));
         }
      }
   }
}
