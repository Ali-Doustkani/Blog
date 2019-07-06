using Blog.Model;
using Blog.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
    [Authorize]
    public class AdministratorController : Controller
    {
        public AdministratorController(PostServices services)
        {
            _services = services;
        }

        private readonly PostServices _services;

        public ViewResult Index() => View(_services.GetPosts());

        public ViewResult Post() => View(_services.Create());

        public IActionResult ViewPost(int id)
        {
            var post = _services.Get(id);
            if (post == null)
                return NotFound();
            return View(nameof(Post), post);
        }

        [ValidateAntiForgeryToken]
        public IActionResult SavePost(PostViewModel post)
        {
            if (!ModelState.IsValid)
                return View(nameof(Post), post);

            var urlTitle = _services.Save(post);
            if (post.Show)
                return RedirectToAction("Post", "Home", new { urlTitle });
            return RedirectToAction(nameof(Index));
        }

        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            _services.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
