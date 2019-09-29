using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Domain.Blogging.Abstractions;
using Blog.Infrastructure;
using Blog.Utils;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
         services.ConfigureApplicationCookie(op =>
         {
            op.SlidingExpiration = false;
            op.ExpireTimeSpan = TimeSpan.FromDays(30);
         });
         services.AddAuthentication(options =>
            {
               options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
               options.Authority = "https://ali-doustkani.auth0.com/";
               options.Audience = "http://localhost:5000/api/developer";
            });
         services.AddMvc(cfg =>
         {
            cfg.Filters.Add<MigrationFilter>();
         });
         services.AddHttpClient();
         services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlogContext>();
         services.AddAutoMapper(GetType().Assembly);
         services.AddTransient<IHtmlProcessor, HtmlProcessor>();
         services.AddTransient<IImageProcessor, CloudImageProcessor>();
         services.AddTransient<ICodeFormatter, HerokuCodeFormatter>();
         services.AddTransient<IFileSystem, FileSystem>();
         services.AddTransient<IDateProvider, DefaultDateProvider>();
         services.AddScoped<ImageContext>();
         services.AddMediatR(GetType().Assembly);
         services.AddSpaStaticFiles(options =>
         {
            options.RootPath = "wwwroot/admin";
         });
      }

      public void Configure(IApplicationBuilder app)
      {
         app.UseStatusCodePagesWithReExecute("/home/error", "?statusCode={0}");

         if (_env.IsProduction())
         {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
               ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });
            app.UseExceptionHandler("/home/error");
            app.UseHsts();
            app.UseHttpsRedirection();
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
         }
         else
         {
            app.UseDeveloperExceptionPage();
         }

         app.UseAuthentication();
         app.UseStaticFiles();
         app.UseSpaStaticFiles();

         app.Map("/newadmin", clientApp =>
         {
            clientApp.UseSpa(spa => { });
         });

         app.UseMvc(cfg =>
         {
            cfg.MapRoute("root", "/", new { controller = "home", action = "index", language = "fa" })
               .MapRoute("langRoot", "{language:regex(^fa|en$)}", new { controller = "home", action = "index" })
               .MapRoute("post", "{language:regex(^fa|en$)}/{urlTitle}", new { controller = "home", action = "post" })
               .MapRoute("about", "about", new { controller = "home", action = "about" });
            cfg.MapRoute("admin", "admin/{action=index}/{id?}", new { controller = "administrator" });
            cfg.MapRoute("default", "{controller}/{action}");
         });
      }
   }
}