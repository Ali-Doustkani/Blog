using Blog.Storage;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests
{
   public class TestContext
   {
      public TestContext()
      {
         _connection = new SqliteConnection("DataSource=:memory:");
         _connection.Open();
         using (var ctx = GetDb())
            ctx.Database.EnsureCreated();
      }

      private readonly SqliteConnection _connection;

      public BlogContext GetDb()
      {
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(_connection);
         return new BlogContext(optionBuilder.Options);
      }
   }
}
