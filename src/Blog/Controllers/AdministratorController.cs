using Blog.Domain;
using Blog.Services.DraftDeleteCommand;
using Blog.Services.DraftListQuery;
using Blog.Services.DraftPreviewQuery;
using Blog.Services.DraftQuery;
using Blog.Services.DraftSaveCommand;
using Blog.Utils;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   [Authorize]
   public class AdministratorController : Controller
   {
      public AdministratorController(IMediator mediator)
      {
         _mediator = mediator;
      }

      private readonly IMediator _mediator;

      public async Task<ViewResult> Index() =>
         View(await _mediator.Send(new DraftListQuery()));

      public ViewResult Post() =>
         View(new DraftSaveCommand());

      public async Task<IActionResult> ViewPost(int id)
      {
         var result = await _mediator.Send(new DraftQuery { Id = id });
         if (result == null)
            return NotFound();
         return View(nameof(Post), result);
      }

      [ValidateAntiForgeryToken]
      public async Task<IActionResult> SavePost(DraftSaveCommand command)
      {
         if (!ModelState.IsValid)
            return View(nameof(Post), command);

         var result = await _mediator.Send(command);
         if (result.Published)
         {
            var lang = command.Language == Language.English ? "en" : "fa";
            return RedirectToAction("Post", "Home", new { language = lang, urlTitle = result.PostUrl });
         }

         return RedirectToAction(nameof(Index));
      }

      [ValidateAntiForgeryToken]
      public async Task<IActionResult> DeletePost(int id)
      {
         await _mediator.Send(new DraftDeleteCommand { Id = id });
         return RedirectToAction(nameof(Index));
      }

      public async Task<IActionResult> Preview(DraftPreviewQuery draft)
      {
         var result = await _mediator.Send(draft);
         if (result.Failed)
         {
            ModelState.AddModelErrors(result.Errors);
            return View();
         }

         ViewData["language"] = result.Post.Language;
         return View("Views/Home/Post.cshtml", result.Post);
      }

      [IgnoreMigration]
      public IActionResult Developer() =>
         View();
   }
}