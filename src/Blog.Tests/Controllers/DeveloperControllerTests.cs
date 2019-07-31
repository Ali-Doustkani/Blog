using Blog.Services.DeveloperStory;
using FluentAssertions;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.Controllers
{
   public class DeveloperControllerTests : IClassFixture<ClientFixture>
   {
      public DeveloperControllerTests(ClientFixture fixture)
      {
         _service = fixture.DeveloperService;
         _client = fixture.Client;
      }

      private readonly HttpClient _client;
      private readonly Mock<IDeveloperService> _service;

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
   }
}
