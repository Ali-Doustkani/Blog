using Blog.CQ.DeveloperQuery;
using Blog.CQ.PostListQuery;
using Blog.CQ.PostQuery;
using Blog.Domain;
using Blog.Utils;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Blog.Controllers
{
   public class HomeController : Controller
   {
      public HomeController(IMediator mediator)
      {
         _mediator = mediator;
      }

      private readonly IMediator _mediator;

      public async Task<ViewResult> Index(string language)
      {
         var lang = Language.English;
         if (string.Equals(language, "fa", StringComparison.OrdinalIgnoreCase))
            lang = Language.Farsi;
         ViewData["language"] = lang;
         return View(await _mediator.Send(new PostListQuery { Language = lang }));
      }

      public async Task<IActionResult> Post(string language, string urlTitle)
      {
         var post = await _mediator.Send(new PostQuery { PostUrl = urlTitle });
         if (post == null)
            return NotFound();

         if (language == "fa" && post.Language != Language.Farsi || language == "en" && post.Language != Language.English)
            return NotFound();

         ViewData["language"] = post.Language;

         return View(post);
      }

      public async Task<IActionResult> About()
      {
         var developer = await _mediator.Send(new DeveloperQuery());
         if (developer == null)
            return NotFound();

         ViewData["language"] = Language.English;

         return View(developer);
      }

      [IgnoreMigration]
      public IActionResult Error(int statusCode = -1)
      {
         ViewData["language"] = Language.English;
         if (statusCode == 404)
            return View("NotFound");
         return View();
      }
   }
}
