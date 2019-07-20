using Blog.Domain;
using Blog.Services;
using Blog.Services.Administrator;
using Blog.Utils;
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

         var result = _services.Save(draft);
         if (result.Failed)
         {
            ModelState.AddModelErrors(result.Problems);
            return View(nameof(Post), draft);
         }

         if (draft.Publish)
         {
            var lang = draft.Language == Language.English ? "en" : "fa";
            return RedirectToAction("Post", "Home", new { language = lang, urlTitle = result.Url });
         }

         return RedirectToAction(nameof(Index));

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