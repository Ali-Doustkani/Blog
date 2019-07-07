using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Formatting.Json;
using Microsoft.Extensions.Logging;

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
            .ConfigureLogging((context, cfg) =>
            {
                cfg.ClearProviders();
                cfg.AddConsole();
                if (context.HostingEnvironment.IsProduction())
                {
                    cfg.AddSerilog(new LoggerConfiguration()
                        .WriteTo
                        .File(new JsonFormatter(), "log/logs.json")
                        .CreateLogger()
                        );
                }
            });
    }
}
