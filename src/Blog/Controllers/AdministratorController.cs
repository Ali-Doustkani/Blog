using Blog.Services;
using Blog.Services.Administrator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [Authorize]
   public class AdministratorController : Controller
   {
      public AdministratorController(Service services)
      {
         _services = services;
      }

      private readonly Service _services;

      public ViewResult Index() => View(_services.GetDrafts());

      public ViewResult Post() => View(_services.Create());

      public IActionResult ViewPost(int id)
      {
         var post = _services.Get(id);
         if (post == null)
            return NotFound();
         return View(nameof(Post), post);
      }

      [ValidateAntiForgeryToken]
      public IActionResult SavePost(DraftEntry draft)
      {
         if (!ModelState.IsValid)
            return View(nameof(Post), draft);
         try
         {
            var result = _services.Save(draft);
            if (draft.Publish)
               return RedirectToAction("Post", "Home", new { urlTitle = result });
            return RedirectToAction(nameof(Index));
         }
         catch (ValidationException ex)
         {
            ModelState.AddModelError(ex.Key, ex.Message);
            return View(nameof(Post), draft);
         }
      }

      [ValidateAntiForgeryToken]
      public IActionResult DeletePost(int id)
      {
         _services.Delete(id);
         return RedirectToAction(nameof(Index));
      }

      public IActionResult Preview(int id)
      {
         var post = _services.GetView(id);
         if (post == null)
            return NotFound();

         ViewData["language"] = post.Language;
         return View("Views/Home/Post.cshtml", post);
      }
   }
}
