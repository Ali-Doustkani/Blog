using AutoMapper;
using Blog.Domain;
using Blog.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

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
            services.AddMvc();
            services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlogContext>();
            services.AddAutoMapper(GetType().Assembly);
            services.AddTransient<Services.Home.Service>();
            services.AddTransient<Services.Administrator.Service>();
            services.AddTransient<IImageContext, ImageContext>();
            services.AddTransient<IFileSystem, FileSystem>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else if (_env.IsProduction())
            {
                app.UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                });
                app.UseExceptionHandler("/home/error");
                app.UseStatusCodePagesWithReExecute("/home/error", "?statusCode={0}");
                app.UseHsts();
                app.UseHttpsRedirection();
                app.UseXXssProtection(options => options.EnabledWithBlockMode());
                app.UseXContentTypeOptions();
            }

            app.MigrateDatabase();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseMvc(cfg =>
            {
                cfg.MapRoute("root", "/", new { controller = "home", action = "index" })
                   .MapRoute("lang", "blog/{language=en}", new { controller = "home", action = "index" })
                   .MapRoute("post", "blog/post/{urlTitle}", new { controller = "home", action = "post" })
                   .MapRoute("about", "about", new { controller = "home", action = "about" });

                cfg.MapRoute("admin", "admin/{action}/{id?}", new { controller = "Administrator" });

                cfg.MapRoute("default", "{controller}/{action}");
            });
        }
    }
}
