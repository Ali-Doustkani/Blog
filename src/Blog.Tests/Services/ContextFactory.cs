using Blog.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.Services
{
   public class ContextFactory
   {
      public ContextFactory()
      {
         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(connection);
         _options = optionBuilder.Options;
         using (var ctx = new BlogContext(_options))
            ctx.Database.EnsureCreated();
      }

      private readonly DbContextOptions _options;

      public BlogContext Create() =>
         new BlogContext(_options);
   }
}
