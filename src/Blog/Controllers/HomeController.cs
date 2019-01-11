using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blog.Controllers
{
    public class HomeController : Controller
    {
        public HomeController(BlogContext context)
        {
            _context = context;
        }

        private readonly BlogContext _context;

        public ViewResult Index(string language)
        {
            var lang = Language.English;
            if (string.Equals(language, "fa", StringComparison.OrdinalIgnoreCase))
                lang = Language.Farsi;

            ViewData["language"] = lang;

            var posts = _context.Posts.Where(x => x.Language == lang);
            if (!User.Identity.IsAuthenticated)
                posts = posts.Where(x => x.Show);
            return View(posts.ToList());
        }

        public IActionResult Post(string title)
        {
            var post = _context.Posts.SingleOrDefault(x => x.Title == title);
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
