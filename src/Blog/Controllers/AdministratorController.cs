﻿using Blog.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Blog.Controllers
{
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

        public ViewResult Post()
        {
            var newPost = new PostViewModel
            {
                PublishDate = DateTime.Now
            };
            return View(newPost);
        }

        public IActionResult ViewPost(int id)
        {
            var post = _context.Posts.Find(id);
            if (post == null)
                return NotFound();
            return View(nameof(Post), post);
        }

        [ValidateAntiForgeryToken]
        public IActionResult SavePost(PostViewModel post)
        {
            if (!ModelState.IsValid)
                return View(nameof(Post), post);

            post.PopulateUrlTitle();
            post.DisplayContent = Article.Decorate(post.MarkedContent);

            if (post.Id == 0)
                _context.Posts.Add(post);
            else
            {
                _context.Posts.Attach(post);
                _context.Entry(post).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            }
            _context.SaveChanges();
            if (post.Show)
                return RedirectToAction("Post", "Home", new { urlTitle = post.UrlTitle });
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
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
