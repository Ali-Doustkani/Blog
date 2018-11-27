﻿using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blog.Controllers
{
    [Route("admin")]
    [Authorize]
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

        [Route("post/{id}")]
        public IActionResult Post(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            return View(post);
        }

        [Route("post/save")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Save(Post post)
        {
            if (post.Id == 0)
                _context.Posts.Add(post);
            else
            {
                _context.Posts.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _context.SaveChanges();
            if (post.Show)
                return RedirectToAction("Post", "Home", new { id = post.Id });
            return RedirectToAction(nameof(Index));
        }

        [Route("post/delete/{id}")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            _context.Posts.Remove(post);
            _context.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
