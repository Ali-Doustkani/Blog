﻿using Blog.Model;
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

        public IActionResult Post(int id)
        {
            var post = _context.Posts.SingleOrDefault(x => x.Id == id && x.Show);
            if (post == null)
                return NotFound();
            return View(post);
        }

        public ViewResult About()
        {
            return View();
        }
    }
}
