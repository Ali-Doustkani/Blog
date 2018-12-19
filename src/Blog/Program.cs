﻿using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;

namespace Blog
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseKestrel(options =>
                {
                    options.AddServerHeader = false;
                })
                .ConfigureAppConfiguration(builder =>
                {
                    builder.AddJsonFile("secrets.json", true);
                })
                .UseSerilog((context, cfg) =>
                {
                    if (context.HostingEnvironment.IsProduction())
                        cfg.WriteTo.File("logs.txt");
                });
    }
}
