using Blog.Domain;
using Blog.Domain.Blogging;
using Blog.Services.Administrator;
using Blog.Storage;
using Blog.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Controllers
{
   [Authorize]
   public class AdministratorController : Controller
   {
      public AdministratorController(IAdminServices services,
         DraftService draftService,
         BlogContext blogContext,
         ImageContext imageContext,
         IDateProvider dateProvider,
         IHtmlProcessor htmlProcessor,
         IStorageState storageState)
      {
         _services = services;
         _draftService = draftService;
         _context = blogContext;
         _imageContext = imageContext;
         _dateProvider = dateProvider;
         _htmlProcessor = htmlProcessor;
         _storageState = storageState;
      }

      private readonly IAdminServices _services;
      private readonly DraftService _draftService;
      private readonly BlogContext _context;
      private readonly ImageContext _imageContext;
      private readonly IDateProvider _dateProvider;
      private readonly IHtmlProcessor _htmlProcessor;
      private readonly IStorageState _storageState;

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

         var command = new DraftUpdateCommand
         {
            Content = draft.Content,
            EnglishUrl = draft.EnglishUrl,
            Language = draft.Language,
            Summary = draft.Summary,
            Tags = draft.Tags,
            Title = draft.Title,
            Id = draft.Id
         };

         var draftResult = _draftService.Save(command);
         if (draftResult.Failed)
         {
            ModelState.AddModelErrors(draftResult.Errors);
            return View(nameof(Post), draft);
         }

         _context.SaveChanges();
         _imageContext.SaveChanges();

         if (draft.Publish)
         {
            var postResult = draftResult.Draft.Publish(_dateProvider, _htmlProcessor, _storageState);
            if (postResult.Failed)
            {
               ModelState.AddModelErrors(postResult.Errors);
               return View(nameof(Post), draft);
            }

            _context.SaveChanges();

            var lang = draft.Language == Language.English ? "en" : "fa";
            return RedirectToAction("Post", "Home", new { language = lang, urlTitle = postResult.Post.Url });

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

      [IgnoreMigration]
      public IActionResult Developer() =>
         View();
   }
}