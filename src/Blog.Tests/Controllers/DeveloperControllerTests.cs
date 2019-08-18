using Blog.Services.DeveloperStory;
using FluentAssertions;
using FluentAssertions.Json;
using Moq;
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
         _service = fixture.DeveloperService;
         _client = fixture.Client;
      }

      private readonly HttpClient _client;
      private readonly Mock<IDeveloperServices> _service;

      [Fact]
      public async Task Return_204_when_no_developer_is_available()
      {
         _service
            .Setup(x => x.Get())
            .Returns((DeveloperEntry)null);

         using (var response = await _client.GetAsync("/api/developer"))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.NoContent);
         }
      }

      [Fact]
      public async Task Return_200_when_developer_is_available()
      {
         _service
            .Setup(x => x.Get())
            .Returns(new DeveloperEntry());

         using (var response = await _client.GetAsync("/api/developer"))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.OK);
         }
      }

      [Fact]
      public async Task Return_201_when_new_developer_is_added()
      {
         _service
            .Setup(x => x.Save(It.IsAny<DeveloperEntry>()))
            .Returns(new SaveResult(Status.Created, new[] { 1 }, new[] { 1 }));

         var developer = new
         {
            Summary = "a web programmer",
            Skills = "C#, HTML"
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.Created);

            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 1 }
            });

            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Return_200_when_developer_is_updated()
      {
         _service
            .Setup(x => x.Save(It.IsAny<DeveloperEntry>()))
            .Returns(new SaveResult(Status.Updated, new[] { 1 }, new[] { 1 }));

         var developer = new
         {
            Summary = "a web programmer",
            Skills = "C#, HTML"
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode
               .Should()
               .Be(HttpStatusCode.OK);

            var result = JToken.Parse(await response.Content.ReadAsStringAsync());
            var expected = JsonConvert.SerializeObject(new
            {
               experiences = new[] { 1 }
            });
            result.Should().BeEquivalentTo(expected);
         }
      }

      [Fact]
      public async Task Return_400_when_developer_is_not_valid()
      {
         var developer = new
         {
            Experiences = new[]
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
               { "Skills", new[] {"The Skills field is required."}},
               { "Summary", new[] {"The Summary field is required."}},
               { "Experiences[0].Company", new[] {"The Company field is required."}},
               { "Experiences[0].Content", new[] {"The Content field is required."}},
               { "Experiences[0].EndDate", new[] {"The EndDate field is required."}},
               { "Experiences[0].Position", new[] {"The Position field is required."}},
               { "Experiences[0].StartDate", new[] {"The StartDate field is required."}},
            };

            result.Should().BeEquivalentTo(JsonConvert.SerializeObject(dic));
         }
      }
   }
}
