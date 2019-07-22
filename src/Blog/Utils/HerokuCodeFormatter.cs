using Blog.Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Blog.Utils
{
   public class HerokuCodeFormatter : ICodeFormatter
   {
      public HerokuCodeFormatter(ILogger<HerokuCodeFormatter> logger)
      {
         _logger = logger;
      }

      private readonly ILogger _logger;

      public string Format(string language, string code)
      {
         try
         {
            var url = "https://beautifycode.herokuapp.com/highlight";
            var client = new HttpClient();
            var content = JsonConvert.SerializeObject(new { language, code });
            var response = client.PostAsync(url, new StringContent(content, Encoding.UTF8, "application/json")).Result;
            _logger.LogInformation("Beautify Endpoint {0}, Status {1}", url, response.StatusCode);
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
         }
         catch (Exception ex)
         {
            throw new CodeFormatException(ex);
         }
      }
   }
}
