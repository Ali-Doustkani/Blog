using Blog.CQ.DraftDeleteCommand;
using Blog.CQ.DraftListQuery;
using Blog.CQ.DraftQuery;
using Blog.CQ.DraftSaveCommand;
using Blog.CQ.PreviewQuery;
using Blog.Domain;
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
         var post = await _mediator.Send(draft);
         ViewData["language"] = post.Language;
         return View("Views/Home/Post.cshtml", post);
      }

      [IgnoreMigration]
      public IActionResult Developer() =>
         View();
   }
}