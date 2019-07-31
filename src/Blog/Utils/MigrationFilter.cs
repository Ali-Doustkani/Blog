using Blog.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Blog.Utils
{
   public class MigrationFilter : IResourceFilter
   {
      public MigrationFilter(BlogContext dbContext, ILogger<MigrationFilter> logger, IConfiguration configuration)
      {
         _dbContext = dbContext;
         _logger = logger;
         _configuration = configuration;
      }

      private readonly BlogContext _dbContext;
      private readonly ILogger _logger;
      private readonly IConfiguration _configuration;

      public void OnResourceExecuted(ResourceExecutedContext context) { }

      public void OnResourceExecuting(ResourceExecutingContext context)
      {
         if (context.Filters.OfType<IgnoreMigrationAttribute>().Any())
            return;

         if (_dbContext.Database.ProviderName.Contains("Sqlite"))
            return;

         try
         {
            _dbContext.Database.Migrate();
            SeedData();
         }
         catch (Exception ex)
         {
            _logger.LogError(ex, ex.Message);
            throw;
         }
      }

      private void SeedData()
      {
         if (_dbContext.Users.Any())
            return;
         var admin = new IdentityUser();
         admin.UserName = _configuration["seed:username"];
         admin.NormalizedUserName = admin.UserName.ToUpper();
         admin.PasswordHash = _configuration["seed:passwordHash"];
         admin.LockoutEnabled = true;
         admin.SecurityStamp = "2ENRK26CZS2IAUJQHCG5PLZ7QDO3U7KX";
         _dbContext.Users.Add(admin);
         _dbContext.SaveChanges();

      }
   }
}
