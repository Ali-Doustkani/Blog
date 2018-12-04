using Blog.Controllers;
using Blog.Model;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHsts(options =>
            {
                options.MaxAge = TimeSpan.FromDays(365);
                options.IncludeSubDomains = true;
                options.Preload = true;
            });
            services.AddDbContext<BlogContext>(options =>
            {
                options.UseSqlServer(_configuration.GetConnectionString("Blog"));
            });
            services.AddMvc(options =>
            {
                options.Filters.Add(new RequireHttpsAttribute());
            });
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlogContext>();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            app.UseStaticFiles();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
            app.UseAuthentication();
            app.UseMvc(cfg =>
            {
                cfg.MapRoute("root1", "/", Home(nameof(HomeController.Index)))
                   .MapRoute("root2", "blog", Home(nameof(HomeController.Index)))
                   .MapRoute("post", "blog/post/{id}", Home(nameof(HomeController.Post)))
                   .MapRoute("about", "about", Home(nameof(HomeController.About)));

                cfg.MapRoute("admin", "admin/{action}/{id?}", new { controller = Extensions.NameOf<AdministratorController>() });

                cfg.MapRoute("default", "{controller}/{action}");
            });
        }

        private object Home(string actionName)
        {
            var controllerName = Extensions.NameOf<HomeController>();
            return new { controller = controllerName, action = actionName };
        }
    }
}
