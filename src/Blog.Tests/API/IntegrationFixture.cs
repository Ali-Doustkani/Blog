using Blog.Domain;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Blog.Tests.API
{
   public class IntegrationFixture : IDisposable
   {
      public IntegrationFixture()
      {
         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(connection);
         using (var ctx = new BlogContext(optionBuilder.Options))
            ctx.Database.EnsureCreated();

         var builder = new WebHostBuilder()
            .UseStartup<Startup>()
            .UseEnvironment("Testing")
            .ConfigureServices(services =>
            {
               services.AddTransient(s => new BlogContext(optionBuilder.Options));
            });

         _server = new TestServer(builder);
         Client = _server.CreateClient();
      }

      private readonly TestServer _server;

      public HttpClient Client { get; }

      public void Dispose()
      {
         Client.Dispose();
         _server.Dispose();
      }
   }
}
