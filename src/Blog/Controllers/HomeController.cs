using Blog.Domain.Blogging;
using Blog.Services.Home;
using Blog.Utils;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.Controllers
{
   public class HomeController : Controller
   {
      public HomeController(Service services)
      {
         _services = services;
      }

      private readonly Service _services;

      public ViewResult Index(string language)
      {
         var lang = Language.English;
         if (string.Equals(language, "fa", StringComparison.OrdinalIgnoreCase))
            lang = Language.Farsi;
         ViewData["language"] = lang;
         return View(_services.GetPosts(lang));
      }

      public IActionResult Post(string language, string urlTitle)
      {
         var post = _services.Get(urlTitle);
         if (post == null)
            return NotFound();

         if (language == "fa" && post.Language != Language.Farsi || language == "en" && post.Language != Language.English)
            return NotFound();

         ViewData["language"] = post.Language;

         return View(post);
      }

      [IgnoreMigration]
      public ViewResult About()
      {
         ViewData["language"] = Language.English;
         return View();
      }

      [IgnoreMigration]
      public IActionResult Error(int statusCode = -1)
      {
         ViewData["language"] = Language.English;
         if (statusCode == 404)
            return View("NotFound");
         return View();
      }
   }
}
