using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using System;

namespace Blog.Controllers
{
    [Route("admin")]
    public class AdministratorController : Controller
    {
        public AdministratorController(BlogContext context)
        {
            _context = context;
        }

        private readonly BlogContext _context;

        [Route("post")]
        public ViewResult Post()
        {
            var newPost = new Post
            {
                PublishDate = DateTime.Now
            };
            return View(newPost);
        }

        [Route("post/save")]
        [HttpPost]
        public IActionResult Save(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
            return Redirect("~/blog/post/" + post.Id.ToString());
        }
    }
}
