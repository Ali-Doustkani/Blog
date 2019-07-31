﻿using AutoMapper;
using Blog.Domain;
using Blog.Domain.Blogging;
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
         services.ConfigureApplicationCookie(op =>
         {
            op.SlidingExpiration = false;
            op.ExpireTimeSpan = TimeSpan.FromDays(30);
         });
         services.AddMvc();
         services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<BlogContext>();
         services.AddAutoMapper(GetType().Assembly);
         services.AddTransient<Services.Home.Service>();
         services.AddTransient<Services.Administrator.Service>();
         services.AddTransient<Services.DeveloperStory.Service>();
         services.AddTransient<IImageContext, ImageContext>();
         services.AddTransient<IFileSystem, FileSystem>();
         services.AddTransient<DraftValidator>();
         services.AddTransient<ICodeFormatter, HerokuCodeFormatter>();
         services.AddTransient<IImageProcessor, CloudImageProcessor>();
         services.AddTransient<Services.Administrator.DraftSaveCommand>();
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
            app.UseXXssProtection(options => options.EnabledWithBlockMode());
            app.UseXContentTypeOptions();
         }
         else
         {
            app.UseDeveloperExceptionPage();
         }

         app.MigrateDatabase();
         app.UseStaticFiles();
         app.UseAuthentication();
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