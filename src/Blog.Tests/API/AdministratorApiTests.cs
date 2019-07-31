using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Blog.Tests.API
{
   public class AdministratorApiTests : IClassFixture<IntegrationFixture>
   {
      public AdministratorApiTests(IntegrationFixture fixture)
      {
         _client = fixture.Client;
      }

      private readonly HttpClient _client;

      [Fact]
      public async Task Get_204_when_there_is_no_developer()
      {
         using (var response = await _client.GetAsync("/api/developer"))
         {
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);
         }
      }

      [Fact]
      public async Task Save_developer_and_get_it()
      {
         var developer = new
         {
            Summary = "The best developer ever!",
            Experiences = new[]
            {
               new
               {
                  Company = "Parmis",
                  Position = "C# Developer",
                  Content = "System Architect",
                  StartDate = "2016-01-20",
                  EndDate = "2017-01-01"
               },
               new
               {
                  Company = "Lodgify",
                  Position = "Javascript Developer",
                  Content = "Backend Developer",
                  StartDate = "2018-01-01",
                  EndDate = "2019-01-01"
               }
            }
         };

         using (var response = await _client.PutAsJsonAsync("/api/developer", developer))
         {
            response.StatusCode.Should().Be(HttpStatusCode.Created);
         }
      }
   }
}
