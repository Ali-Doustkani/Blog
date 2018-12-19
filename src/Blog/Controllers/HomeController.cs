using Blog.Model;
using Microsoft.AspNetCore.Mvc;
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

        public ViewResult Index()
        {
            return View(_context.Posts.Where(x => x.Show).ToList());
        }

        public IActionResult Post(string title)
        {
            var post = _context.Posts.SingleOrDefault(x => x.Title == title);
            if (post == null)
                return NotFound();

            if (!post.Show && !HttpContext.User.Identity.IsAuthenticated)
                return Challenge();

            return View(post);
        }

        public ViewResult About()
        {
            return View();
        }
    }
}
