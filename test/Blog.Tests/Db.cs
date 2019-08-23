using Blog.Domain;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests
{
   public static class Db
   {
      public static BlogContext CreateInMemory()
      {
         var connection = new SqliteConnection("DataSource=:memory:");
         connection.Open();
         var optionBuilder = new DbContextOptionsBuilder();
         optionBuilder.UseSqlite(connection);
         var ctx = new BlogContext(optionBuilder.Options);
         ctx.Database.EnsureCreated();
         return ctx;
      }
   }
}
