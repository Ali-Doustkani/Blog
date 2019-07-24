using Blog.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;

namespace Blog.Utils
{
   public class CloudImageProcessor : IImageProcessor
   {
      public CloudImageProcessor(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
      {
         _token = configuration["cloudImage:token"];
         _httpContextAccessor = httpContextAccessor;
      }

      private readonly string _token;
      private readonly IHttpContextAccessor _httpContextAccessor;

      public string Minimize(string originalImage)
      {
         try
         {
            var req = _httpContextAccessor.HttpContext.Request;
            var client = new HttpClient();
            var result = client.GetAsync($"https://{_token}.cloudimg.io/width/25/none/{req.Scheme}://{req.Host.Host}:{req.Host.Port}{originalImage}").Result;
            result.EnsureSuccessStatusCode();
            var base64 = Convert.ToBase64String(result.Content.ReadAsByteArrayAsync().Result);
            return DataUrl(originalImage, base64);
         }
         catch (Exception ex)
         {
            throw new ServiceDependencyException("Processing image failed.", ex);
         }
      }

      private string DataUrl(string file, string base64)
      {
         var types = new Dictionary<string, string>
         {
            { ".jpg", "jpeg" },
            {".png", "png" },
            {".gif", "gif" }
         };
         var type = types[Path.GetExtension(file).ToLower()];
         return $"data:image/{type};base64,{base64}";
      }
   }
}
