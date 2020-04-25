using Microsoft.EntityFrameworkCore;

namespace Blog.Infrastructure
{
   public static class Migrator
   {
      public static void Migrate(string server, string database, string username, string password)
      {
          Migrate($"Data Source={server};Initial Catalog={database};User ID={username};Password={password};Connect Timeout=60;");
      }

      public static void MigrateLocalDb()
      {
          Migrate("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=BlogDb;Integrated Security=True;");
      }

      private static void Migrate(string connectionString)
      {
          var optionsBuilder = new DbContextOptionsBuilder();
          optionsBuilder.UseSqlServer(connectionString);
          var context = new BlogContext(optionsBuilder.Options);
          context.Database.Migrate();
      }
   }
}
