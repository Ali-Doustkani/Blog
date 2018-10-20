using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Blog.Controllers
{
    public class HomeController: Controller
    {
        public HomeController(BlogContext context)
        {
            _context = context;
        }

        private readonly BlogContext _context;

        [Route("blog")]
        public ViewResult Index()
        {
            return View(_context.Posts.ToList());
        }

        [Route("blog/post/{id}")]
        public IActionResult Post(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        [Route("")]
        [Route("about")]
        public ViewResult About()
        {
            return View("About");
        }
    }
}
