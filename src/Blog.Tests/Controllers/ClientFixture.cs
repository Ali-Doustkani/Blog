using Blog.Domain;
using Blog.Services.DeveloperStory;
using Blog.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http;

namespace Blog.Tests.Controllers
{
   public class ClientFixture
   {
      public ClientFixture()
      {
         DeveloperService = new Mock<IDeveloperServices>();

         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(connection);
         using (var ctx = new BlogContext(optionBuilder.Options))
            ctx.Database.EnsureCreated();

         var builder = new WebHostBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Testing")
            .UseSetting("ConnectionStrings:Blog", "FakeConnectionString")
            .ConfigureTestServices(services =>
            {
               services.AddTransient(s => DeveloperService.Object);
               services.AddMvc(cfg => cfg.Filters.Add<IgnoreMigrationAttribute>());
            });
         var server = new TestServer(builder);
         Client = server.CreateClient();
      }

      public HttpClient Client { get; }
      public Mock<IDeveloperServices> DeveloperService { get; }
   }
}
