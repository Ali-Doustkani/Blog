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
