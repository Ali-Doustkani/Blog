using Blog.Model;
using Blog.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Westwind.AspNetCore.LiveReload;

namespace Blog
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private readonly IConfiguration _configuration;
        private readonly IHostingEnvironment _env;

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

            if (_env.IsDevelopment())
                services.AddLiveReload();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (_env.IsDevelopment())
            {
                app.UseLiveReload();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/home/error");
                app.UseStatusCodePagesWithReExecute("/home/error", "?statusCode={0}");
                app.UseHsts();
                app.UseXXssProtection(options => options.EnabledWithBlockMode());
                app.UseXContentTypeOptions();
            }

            app.MigrateDatabase();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(cfg =>
            {
                cfg.MapRoute("root", "/", new { controller = "home", action = "index", language = "fa" })
                   .MapRoute("lang", "blog/{language=en}", new { controller = "home", action = "index" })
                   .MapRoute("post", "blog/post/{urlTitle}", new { controller = "home", action = "post" })
                   .MapRoute("about", "about", new { controller = "home", action = "about" });

                cfg.MapRoute("admin", "admin/{action}/{id?}", new { controller = "Administrator" });

                cfg.MapRoute("default", "{controller}/{action}");
            });
        }
    }
}
