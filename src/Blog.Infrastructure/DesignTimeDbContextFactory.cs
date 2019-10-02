using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Blog.Infrastructure
{
   public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<BlogContext>
   {
      public BlogContext CreateDbContext(string[] args)
      {
         var configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();
         var builder = new DbContextOptionsBuilder<BlogContext>();
         var connectionString = configuration.GetConnectionString("Blog");
         builder.UseSqlServer(connectionString);
         return new BlogContext(builder.Options);
      }
   }
}
