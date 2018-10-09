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

        public ViewResult Index()
        {
            return View(_context.Posts.ToList());
        }

        public ViewResult Post(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        public ViewResult About()
        {
            return View("About");
        }
    }
}
