using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
   public static class Migrator
   {
      public static void Migrate(string server, string database, string username, string password)
      {
         var connectionString = $"Data Source={server};Initial Catalog={database};User ID={username};Password={password};Connect Timeout=60;";
         var optionsBuilder = new DbContextOptionsBuilder();
         optionsBuilder.UseSqlServer(connectionString);
         var context = new BlogContext(optionsBuilder.Options);
         context.Database.Migrate();
      }
   }
}
