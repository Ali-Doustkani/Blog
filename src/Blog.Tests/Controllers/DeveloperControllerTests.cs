using Blog.Services.DeveloperStory;
using FluentAssertions;
using FluentAssertions.Json;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
            .Returns(new SaveResult(Status.Created));

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
         }
      }

      [Fact]
      public async Task Return_200_when_developer_is_updated()
      {
         _service
            .Setup(x => x.Save(It.IsAny<DeveloperEntry>()))
            .Returns(new SaveResult(Status.Updated));

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
         }
      }

      [Fact]
      public async Task Return_400_when_developer_is_not_valid()
      {
         var developer = new
         {
            Experiences = new[]
            {
               new{ Id="sdf"}
            }
         };
         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
            var result = JToken.Parse(await response.Content.ReadAsStringAsync());

            var expected = JsonConvert.SerializeObject(new
            {
               title = "Validation",
               errors = new[]
               {
                  new{error="convert", path=new object[]{"Experiences", 0, "Id"}, type="integer"}
               }
            });

            result.Should().BeEquivalentTo(expected);
         }
      }
   }
}
