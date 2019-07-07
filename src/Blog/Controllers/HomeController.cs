using Blog.Domain;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(HomeServices services)
        {
            _services = services;
        }

        private readonly HomeServices _services;

        public ViewResult Index(string language)
        {
            var lang = Language.English;
            if (string.Equals(language, "fa", StringComparison.OrdinalIgnoreCase))
                lang = Language.Farsi;
            ViewData["language"] = lang;
            if (!User.Identity.IsAuthenticated)
                return View(_services.GetVerifiedPosts(lang));
            return View(_services.GetPosts(lang));
        }

        public IActionResult Post(string urlTitle)
        {
            var post = _services.Get(urlTitle);
            if (post == null)
                return NotFound();

            ViewData["language"] = post.Language;

            if (!post.Show && !HttpContext.User.Identity.IsAuthenticated)
                return Challenge();

            return View(post);
        }

        public ViewResult About()
        {
            ViewData["language"] = Language.English;
            return View();
        }

        public IActionResult Error(int statusCode = -1)
        {
            ViewData["language"] = Language.English;
            if (statusCode == 404)
                return View("NotFound");
            return View();
        }
    }
}
