using Blog.Model;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

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

        public ViewResult Index()
        {
            return View(_context.Posts.ToList());
        }

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

        [Route("post/delete/{id}")]
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return Ok();
        }
    }
}
