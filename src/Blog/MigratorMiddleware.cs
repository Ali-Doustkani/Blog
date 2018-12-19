using Blog.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace Blog
{
    public static class MigratorExtension
    {
        public static void MigrateDatabase(this IApplicationBuilder app)
        {
            app.UseMiddleware<MigratorMiddleware>();
        }
    }

    public class MigratorMiddleware
    {
        public MigratorMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        private readonly RequestDelegate next;

        public async Task Invoke(HttpContext httpContext, [FromServices]BlogContext dbContext, [FromServices]IConfiguration configuration)
        {
            dbContext.Database.Migrate();

            if (!dbContext.Users.Any())
            {
                var admin = new IdentityUser();
                admin.UserName = configuration["seed:username"];
                admin.NormalizedUserName = admin.UserName.ToUpper();
                admin.PasswordHash = configuration["seed:passwordHash"];
                admin.LockoutEnabled = true;
                admin.SecurityStamp = "2ENRK26CZS2IAUJQHCG5PLZ7QDO3U7KX";
                dbContext.Users.Add(admin);
                dbContext.SaveChanges();
            }

            await next(httpContext);
        }
    }
}
