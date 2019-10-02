using Blog.Domain;
using Blog.Domain.Blogging.Abstractions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Infrastructure
{
   public class HerokuCodeFormatter : ICodeFormatter
   {
      public HerokuCodeFormatter(IHttpClientFactory httpClientFactory, ILogger<HerokuCodeFormatter> logger)
      {
         _httpClientFactory = httpClientFactory;
         _logger = logger;
      }

      private readonly IHttpClientFactory _httpClientFactory;
      private readonly ILogger _logger;

      public async Task<string> FormatAsync(string language, string code)
      {
         try
         {
            var url = "https://beautifycode.herokuapp.com/highlight";
            using (var client = _httpClientFactory.CreateClient())
            {
               var content = JsonConvert.SerializeObject(new { language, code });
               var response = await client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json"));
               _logger.LogInformation("Beautify Endpoint {0}, Status {1}", url, response.StatusCode);
               response.EnsureSuccessStatusCode();
               return (await response.Content.ReadAsStringAsync()).Trim();
            }
         }
         catch (Exception ex)
         {
            throw new ServiceDependencyException("Formatting code failed.", ex);
         }
      }
   }
}
