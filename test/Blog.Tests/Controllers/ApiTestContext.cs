using Blog.Domain;
using Blog.Storage;
using Blog.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace Blog.Tests.Controllers
{
   public class ApiTestContext
   {
      public ApiTestContext()
      {
         var builder = new WebHostBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Testing")
            .UseSetting("ConnectionStrings:Blog", "FakeConnectionString")
            .ConfigureTestServices(services =>
            {
               services.AddMvc(cfg => cfg.Filters.Add<IgnoreMigrationAttribute>());
               services.AddScoped(s => Database());
            });
         var server = new TestServer(builder);
         Client = server.CreateClient();
      }

      private SqliteConnection _connection;

      public HttpClient Client { get; }

      public void InitializeDatabase()
      {
         _connection = new SqliteConnection("DataSource=:memory:");
         _connection.Open();
         using (var ctx = Database())
            ctx.Database.EnsureCreated();
      }

      private BlogContext Database()
      {
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(_connection);
         return new BlogContext(optionBuilder.Options);
      }
   }
}
